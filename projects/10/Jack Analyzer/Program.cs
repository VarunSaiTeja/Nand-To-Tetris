using System;
using System.Collections.Generic;
using System.IO;

namespace Jack_Analyzer
{
    class Program
    {
        static string jack_name;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("File name : ");
                jack_name = Console.ReadLine();
            }
            else
                jack_name = args[0];

            Tokenizer.Tokenize(jack_name);
            Console.ReadLine();
        }
    }

    class Tokenizer
    {
        static string content = String.Empty, current_token, current_token_type;
        static int position = 0;

        static List<string> Keyword, Symbol;
        public static void Tokenize(String Fname)
        {
            Initilize_Pre_Tokens();
            GetContent(Fname);
            StreamWriter writer = new StreamWriter("Test.xml");
            writer.WriteLine("<tokens>");
            while (position < content.Length)
            {
                Get_Current_Token();
                if (current_token != String.Empty)
                    writer.WriteLine('<' + current_token_type + "> " + current_token + " </" + current_token_type + '>');
            }
            writer.WriteLine("</tokens>");
            writer.Close();
            Console.WriteLine("XML Generated");
        }

        static void Initilize_Pre_Tokens()
        {
            Keyword = new List<string>
            {
                "class",
                "constructor",
                "function",
                "method",
                "field",
                "static",
                "var",
                "int",
                "char",
                "boolean",
                "void",
                "true",
                "false",
                "null",
                "this",
                "let",
                "do",
                "if",
                "else",
                "while",
                "return"
            };
            Symbol = new List<string>
            {
                "{",
                "}",
                "(",
                ")",
                "[",
                "]",
                "",
                ".",
                ",",
                ";",
                "+",
                "-",
                "*",
                "/",
                "&",
                "|",
                "<",
                ">",
                "=",
                "~"
            };
        }
        static void Get_Current_Token()
        {
            current_token = String.Empty;
            var x = content[position];
            if (Char.IsLetter(content[position]) || content[position] == '_')
            {
                while (Char.IsLetter(content[position]) || Char.IsDigit(content[position]) || content[position] == '_')
                {
                    current_token += content[position++];
                }

                if (Keyword.Contains(current_token))
                    current_token_type = "keyword";
                else
                    current_token_type = "identifier";
            }
            else if (Symbol.Contains(content[position].ToString()))
            {
                current_token += content[position++];

                if (current_token == "<")
                    current_token = "&lt;";
                else if (current_token == ">")
                    current_token = "&gt;";
                else if (current_token == '"'.ToString())
                    current_token = "&quot;";
                else if (current_token == "&")
                    current_token = "&amp;";

                current_token_type = "symbol";
            }
            else if (Char.IsDigit(content[position]))
            {
                while (Char.IsDigit(content[position]))
                {
                    current_token += content[position++];
                }
                current_token_type = "integerConstant";
            }
            else if (content[position] == '"')
            {
                position++;
                while (content[position] != '"')
                {
                    current_token += content[position++];
                }

                current_token_type = "stringConstant";
                position++;
            }
            else
                position++;
        }
        static void GetContent(String Fname)
        {
            StreamReader jack_file = new StreamReader(Fname);
            string temp = null;

            while (!jack_file.EndOfStream)
            {
                temp = jack_file.ReadLine().Trim();
                if (!(temp == ""))
                {
                    if (!(temp.StartsWith("//")) || !(temp.Contains("/*")))
                    {
                        if (temp.Contains("//"))
                        {
                            temp = temp.Substring(0, temp.IndexOf("//"));
                            temp = temp.Trim();
                        }
                        else if (temp.Contains("/*"))
                        {
                            temp = temp.Substring(0, temp.IndexOf("/*")).Trim();
                        }
                        content += temp;
                    }
                }
            }

            jack_file.Close();
        }
    }
}

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

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
        static string all_tokens = String.Empty;
        public static void Tokenize(String Fname)
        {
            Remove_Garbage(Fname);
            Console.WriteLine(all_tokens);
        }

        static void Remove_Garbage(String Fname)
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
                        all_tokens += temp;
                    }
                }
            }

            jack_file.Close();
        }
    }
}

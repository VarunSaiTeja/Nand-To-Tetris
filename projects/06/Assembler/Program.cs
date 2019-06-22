using System;
using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    class Program
    {
        static string asm_name;
        static List<string> Lines = new List<string>();
        static Dictionary<string, int> Symbol_Table = new Dictionary<string, int>();
        static Dictionary<int, string> Instructions = new Dictionary<int, string>();
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("File name : ");
                asm_name = Console.ReadLine();
            }
            else
                asm_name = args[0];

            Get_All_Lines();
            Add_Predefiend_Symbols();
            Add_Instructions_and_Label_Symbols();
            Add_Variable_Symbols();
            Translate_Symbols();
            Convert_to_Binary();
            Console.WriteLine("Binary file " + Path.GetFileNameWithoutExtension(asm_name) + ".hack is generated");
            Console.ReadLine();
        }
        static void Convert_to_Binary()
        {
            StreamWriter Hack_File = new StreamWriter(Path.GetFileNameWithoutExtension(asm_name) + ".hack");
            foreach (var item in Instructions)
            {
                if (item.Value.StartsWith('@'))
                    Hack_File.WriteLine(Translate_A_Type(item.Value.Substring(1)));
                else
                    Hack_File.WriteLine(Translate_C_Type(item.Value));
            }
            Hack_File.Close();
        }
        static string Translate_A_Type(string instruction)
        {
            return ("0" + Convert.ToString(Int32.Parse(instruction), 2).PadLeft(15, '0'));
        }
        static string Translate_C_Type(string instruction)
        {
            string binary_instruction = "111";
            binary_instruction += C_Type_Parser.Get_Comp(instruction);
            binary_instruction += C_Type_Parser.Get_Dest(instruction);
            binary_instruction += C_Type_Parser.Get_Jump(instruction);
            return binary_instruction;
        }
        static void Get_All_Lines()
        {
            StreamReader asm_file = new StreamReader(asm_name);
            while (!asm_file.EndOfStream)
            {
                Lines.Add(asm_file.ReadLine().Trim());
            }
            asm_file.Close();
        }
        static void Add_Instructions_and_Label_Symbols()
        {
            int count = 0;
            foreach (var item in Lines)
            {
                if (!(item == ""))
                {
                    if (!item.StartsWith("//"))
                    {
                        string temp = item;
                        if (item.Contains("//"))
                        {
                            temp = item.Substring(0, (item.IndexOf("//"))).Trim();
                        }

                        temp = temp.Replace(" ", String.Empty);
                        if (!temp.StartsWith('('))
                            Instructions.Add(count++, temp);
                        else
                            Symbol_Table.Add(temp.Substring(1, temp.Length - 2), count);
                    }
                }
            }
        }
        static void Add_Variable_Symbols()
        {
            int n = 16;
            foreach (var item in Instructions)
            {
                if (item.Value.StartsWith('@') && !(Char.IsDigit(item.Value[1])))
                {
                    if (!Symbol_Table.ContainsKey(item.Value.Substring(1)))
                    {
                        while (true)
                        {
                            if (!Symbol_Table.ContainsValue(n))
                            {
                                Symbol_Table.Add(item.Value.Substring(1), n++);
                                break;
                            }
                            n++;
                        }
                    }
                }
            }
        }
        static void Translate_Symbols()
        {
            for (int i = 0; i < Instructions.Count; i++)
            {
                if (Instructions[i].StartsWith('@'))
                {
                    if (Symbol_Table.ContainsKey(Instructions[i].Substring(1)))
                    {
                        Instructions[i] = "@" + Symbol_Table[Instructions[i].Substring(1)];
                    }
                }
            }
        }
        static void Add_Predefiend_Symbols()
        {
            Symbol_Table.Add("R0", 0);
            Symbol_Table.Add("R1", 1);
            Symbol_Table.Add("R2", 2);
            Symbol_Table.Add("R3", 3);
            Symbol_Table.Add("R4", 4);
            Symbol_Table.Add("R5", 5);
            Symbol_Table.Add("R6", 6);
            Symbol_Table.Add("R7", 7);
            Symbol_Table.Add("R8", 8);
            Symbol_Table.Add("R9", 9);
            Symbol_Table.Add("R10", 10);
            Symbol_Table.Add("R11", 11);
            Symbol_Table.Add("R12", 12);
            Symbol_Table.Add("R13", 13);
            Symbol_Table.Add("R14", 14);
            Symbol_Table.Add("R15", 15);
            Symbol_Table.Add("SCREEN", 16384);
            Symbol_Table.Add("KBD", 24576);
            Symbol_Table.Add("SP", 0);
            Symbol_Table.Add("LCL", 1);
            Symbol_Table.Add("ARG", 2);
            Symbol_Table.Add("THIS", 3);
            Symbol_Table.Add("THAT", 4);
        }
    }

    class C_Type_Parser
    {
        static Dictionary<string, string> Comp_to_Binary = new Dictionary<string, string>
        {
            { "0", "0101010" },
            { "1", "0111111" },
            { "-1", "0111010" },
            { "D", "0001100" },
            { "A", "0110000" },
            { "!D", "0001101" },
            { "!A", "0110001" },
            { "-D", "0001111" },
            { "-A", "0110011" },
            { "D+1", "0011111" },
            { "A+1", "0110111" },
            { "D-1", "0001110" },
            { "A-1", "0110010" },
            { "D+A", "0000010" },
            { "D-A", "0010011" },
            { "A-D", "0000111" },
            { "D&A", "0000000" },
            { "D|A", "0010101" },
            { "M", "1110000" },
            { "!M", "1110001" },
            { "-M", "1110011" },
            { "M+1", "1110111" },
            { "M-1", "1110010" },
            { "D+M", "1000010" },
            { "D-M", "1010011" },
            { "M-D", "1000111" },
            { "D&M", "1000000" },
            { "D|M", "1010101" }
        };
        static Dictionary<string, string> Dest_to_Binary = new Dictionary<string, string>
        {
            { "null", "000" },
            { "M", "001" },
            { "D", "010" },
            { "MD", "011" },
            { "A", "100" },
            { "AM", "101" },
            { "AD", "110" },
            { "AMD", "111" }
        };
        static Dictionary<string, string> Jump_to_Binary = new Dictionary<string, string>
        {
            { "null", "000" },
            { "JGT", "001" },
            { "JEQ", "010" },
            { "JGE", "011" },
            { "JLT", "100" },
            { "JNE", "101" },
            { "JLE", "110" },
            { "JMP", "111" }
        };
        public static string Get_Dest(string instruction)
        {
            if (instruction.Contains('='))
                return Dest_to_Binary[instruction.Substring(0, instruction.IndexOf('='))];
            else
                return Dest_to_Binary["null"];
        }
        public static string Get_Comp(string instruction)
        {
            if (instruction.Contains('='))
                return Comp_to_Binary[instruction.Substring(instruction.IndexOf('=') + 1)];
            else
                return Comp_to_Binary[instruction[0].ToString()];
        }
        public static string Get_Jump(string instruction)
        {
            if (instruction.Contains(';'))
                return Jump_to_Binary[instruction.Substring(instruction.IndexOf(';') + 1)];
            else
                return Jump_to_Binary["null"];
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    class Program
    {
        static List<string> Lines = new List<string>();
        static Dictionary<string, int> Symbol_Table = new Dictionary<string, int>();
        static Dictionary<int, string> Instructions = new Dictionary<int, string>();
        static void Main(string[] args)
        {
            string asm_name;

            if (args.Length == 0)
            {
                Console.Write("File name : ");
                asm_name = Console.ReadLine();
            }
            else
            {
                asm_name = args[0];
            }

            Get_All_Lines(asm_name);
            Add_Predefiend_Symbols();
            Add_Instructions_and_Label_Symbols();
            Console.ReadLine();
        }

        static void Get_All_Lines(string asm_name)
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

                        if (!temp.StartsWith('('))
                            Instructions.Add(count++, temp);
                        else
                            Symbol_Table.Add(temp.Substring(1, temp.Length - 2), count);
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    class Program
    {
        static List<string> Lines = new List<string>();
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
    }
}

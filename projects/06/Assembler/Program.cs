using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    class Program
    {
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
            Console.ReadLine();
        }
    }
}

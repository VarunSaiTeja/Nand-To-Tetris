using System;

namespace VM_Translator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("File name : ");
                vm_name = Console.ReadLine();
            }
            else
                vm_name = args[0];

            Console.ReadLine();
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;

namespace VM_Translator
{
    class Program
    {
        static string vm_name;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("File name : ");
                vm_name = Console.ReadLine();
            }
            else
                vm_name = args[0];

            Generate_Assembly();
            Console.ReadLine();
        }
        static string Translate(string[] pieces)
        {
            if (pieces.Length == 3)
            {
                if (pieces[0] == "push")
                    return Push_Command(pieces[1], pieces[2]);
                else
                    return Pop_Command(pieces[1], pieces[2]);
            }

            return String.Empty;
        }
        static string Push_Command(string segment, string index)
        {
            if (segment == "constant")
            {
                return (
                    "@" + index + "\n" +
                    "D=A\n" +
                    "@SP\n" +
                    "A=M\n" +
                    "M=D\n" +
                    "@SP\n" +
                    "M=M+1");
            }
            else if (segment == "static")
            {
                return (
                    "@" + Path.GetFileNameWithoutExtension(vm_name) + "." + index + "\n" +
                    "D=M\n" +
                    "@SP\n" +
                    "A=M\n" +
                    "M=D\n" +
                    "@SP\n" +
                    "M=M+1"
                );
            }
            else
            {
                string segment_code = Get_Segment_Code(segment);

                return (
                        "@" + segment_code + "\n" +
                        "D=A\n" +
                        "@" + index + "\n" +
                        "A=D+A\n" +
                        "D=M\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=D\n" +
                        "@SP\n" +
                        "M=M+1"
                    );
            }
        }
        static string Pop_Command(string segment, string index)
        {
            if (segment == "static")
            {
                return (
                    "@SP\n" +
                    "AM=M-1\n" +
                    "D=M\n" +
                    "@" + Path.GetFileNameWithoutExtension(vm_name) + "." + index + "\n" +
                    "M=D"
                );
            }
            else
            {
                string segment_code = Get_Segment_Code(segment);
                return (
                    "@" + segment_code + "\n" +
                    "D=M\n" +
                    "@" + index + "\n" +
                    "D=D+A\n" +
                    "@R13\n" +
                    "M=D\n" +
                    "@SP\n" +
                    "AM=M-1\n" +
                    "D=M\n" +
                    "@R13\n" +
                    "A=M\n" +
                    "M=D"
                );
            }
        }
        static string Get_Segment_Code(string segment)
        {
            switch (segment)
            {
                case "local":
                    return "LCL";
                case "argument":
                    return "ARG";
                case "this":
                    return "THIS";
                case "that":
                    return "THAT";
                case "temp":
                    return "R5";
                case "pointer":
                    return "R3";
                default:
                    return String.Empty;
            }
        }
        static void Generate_Assembly()
        {
            StreamReader vm_file = new StreamReader(vm_name);
            StreamWriter asm_file = new StreamWriter(Path.GetFileNameWithoutExtension(vm_name) + ".asm");
            string temp = null;
            while (!vm_file.EndOfStream)
            {
                temp = vm_file.ReadLine().Trim();
                if (!(temp == ""))
                {
                    if (!(temp.StartsWith("//")))
                    {
                        asm_file.WriteLine("//" + temp);
                        asm_file.WriteLine(Translate(temp.Split(" ")));
                    }
                }
            }
            vm_file.Close();
            asm_file.Close();
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;

namespace VM_Translator
{
    class Program
    {
        static string vm_name;
        static int label_counter = 0, return_counter = 0;
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
            Console.WriteLine("Assembly file " + Path.GetFileNameWithoutExtension(vm_name) + ".asm is generated");
            Console.ReadLine();
        }
        static string Translate(string[] pieces)
        {
            if (pieces.Length == 3)
            {
                if (pieces[0] == "push")
                    return Push_Command(pieces[1], pieces[2]);
                else if (pieces[0] == "pop")
                    return Pop_Command(pieces[1], pieces[2]);
                else if (pieces[0] == "function")
                    return Function_Command(pieces[1], Convert.ToInt16(pieces[2]));
                else if (pieces[0] == "call")
                {
                    return Call_Command(pieces[1], Convert.ToInt16(pieces[2]));
                }
            }
            else if (pieces.Length == 2)
            {
                if (pieces[0] == "label")
                    return '(' + pieces[1] + ')';
                else if (pieces[0] == "if-goto")
                    return If_goto_Command(pieces[1]);
                else if (pieces[0] == "goto")
                    return Goto_Command(pieces[1]);
            }
            else if (pieces.Length == 1)
            {
                if (pieces[0] == "return")
                    return Return_Command();
                else
                    return Arithmetic_Logical_Command(pieces[0]);
            }

            return String.Empty;
        }
        static string Call_Command(string functionName, int argsCount)
        {
            string return_label = "Return_Label_" + (++return_counter).ToString();
            return (
                "@" + return_label + '\n' +
                "D=A\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "@LCL\n" +
                "D=M\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "@ARG\n" +
                "D=M\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "@THIS\n" +
                "D=M\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "@THAT\n" +
                "D=M\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "@SP\n" +
                "D=M\n" +
                "@" + argsCount + '\n' +
                "D=D-A\n" +
                "@5\n" +
                "D=D-A\n" +
                "@ARG\n" +
                "M=D\n" +
                "@SP\n" +
                "D=M\n" +
                "@LCL\n" +
                "M=D\n" +
                "@" + functionName + '\n' +
                "0;JMP\n" +
                '(' + return_label + ')'
            );
        }
        static string Function_Command(string functionName, int variableCount)
        {
            string output = '(' + functionName + ')';
            for (int i = 0; i < variableCount; i++)
            {
                output += (
                    "\n" +
                    "@0\n" +
                    "D=A\n" +
                    "@SP\n" +
                    "A=M\n" +
                    "M=D\n" +
                    "@SP\n" +
                    "M=M+1"
                );
            }
            return output;
        }
        static string Return_Command()
        {
            return (
                "@LCL\n" +
                "D=M\n" +
                "@R13\n" +
                "M=D\n" +
                "@5\n" +
                "A=D-A\n" +
                "D=M\n" +
                "@R14\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M-1\n" +
                "@SP\n" +
                "A=M\n" +
                "D=M\n" +
                "@ARG\n" +
                "A=M\n" +
                "M=D\n" +
                "@ARG\n" +
                "D=M\n" +
                "@R0\n" +
                "M=D+1\n" +
                "@R13\n" +
                "AMD=M-1\n" +
                "D=M\n" +
                "@THAT\n" +
                "M=D\n" +
                "@R13\n" +
                "AMD=M-1\n" +
                "D=M\n" +
                "@THIS\n" +
                "M=D\n" +
                "@R13\n" +
                "AMD=M-1\n" +
                "D=M\n" +
                "@ARG\n" +
                "M=D\n" +
                "@R13\n" +
                "AMD=M-1\n" +
                "D=M\n" +
                "@LCL\n" +
                "M=D\n" +
                "@R14\n" +
                "A=M\n" +
                "0;JMP"
            );
        }
        static string Goto_Command(string label)
        {
            return (
                "@" + label + '\n' +
                "0;JMP"
            );
        }
        static string If_goto_Command(string label)
        {
            return (
                "@SP\n" +
                "AM=M-1\n" +
                "D=M\n" +
                '@' + label + '\n' +
                "D;JNE"
            );
        }
        static string Arithmetic_Logical_Command(string operation)
        {
            switch (operation)
            {
                case "add":
                    return (
                        "@SP\n" +
                        "AM=M-1\n" +
                        "D=M\n" +
                        "A=A-1\n" +
                        "M=D+M"
                    );
                case "sub":
                    return (
                        "@SP\n" +
                        "AM=M-1\n" +
                        "D=M\n" +
                        "A=A-1\n" +
                        "M=M-D"
                    );
                case "and":
                    return (
                            "@SP\n" +
                            "AM=M-1\n" +
                            "D=M\n" +
                            "A=A-1\n" +
                            "M=D&M"
                        );
                case "or":
                    return (
                        "@SP\n" +
                        "AM=M-1\n" +
                        "D=M\n" +
                        "A=A-1\n" +
                        "M=D|M"
                    );
                case "neg":
                    return (
                        "@SP\n" +
                        "A=M-1\n" +
                        "D=0\n" +
                        "M=D-M"
                    );
                case "not":
                    return (
                        "@SP\n" +
                        "A=M-1\n" +
                        "M=!M"
                    );
                case "eq":
                    return Jump_Command("JEQ");
                case "lt":
                    return Jump_Command("JLT");
                case "gt":
                    return Jump_Command("JGT");
                default:
                    return String.Empty;
            }
        }
        static string Jump_Command(string condition)
        {
            label_counter++;
            return (
                "@SP\n" +
                "AM=M-1\n" +
                "D=M\n" +
                "A=A-1\n" +
                "D=M-D\n" +
                "@TrueResult" + label_counter + "\n" +
                "D;" + condition + "\n" +
                "@SP\n" +
                "A=M-1\n" +
                "M=0\n" +
                "@Continue" + label_counter + "\n" +
                "0;JMP\n" +
                "(TrueResult" + label_counter + ")\n" +
                "@SP\n" +
                "A=M-1\n" +
                "M=-1\n" +
                "(Continue" + label_counter + ")"
            );
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
            else if (segment == "temp")
            {
                return (
                    "@R5\n" +
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
            else if (segment == "pointer")
            {
                string segment_code = (index == "0") ? "THIS" : "THAT";
                return (
                    "@" + segment_code + "\n" +
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
                        "D=M\n" +
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
            else if (segment == "temp")
            {
                return (
                    "@R5\n" +
                    "D=A\n" +
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
            else if (segment == "pointer")
            {
                string segment_code = (index == "0") ? "THIS" : "THAT";
                return (
                    "@" + segment_code + "\n" +
                    "D=A\n" +
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
                default:
                    return String.Empty;
            }
        }
        static void Generate_Assembly()
        {
            StreamReader vm_file = new StreamReader(vm_name);
            StreamWriter asm_file = new StreamWriter(Path.ChangeExtension(vm_name, "asm"));
            string temp = null;
            while (!vm_file.EndOfStream)
            {
                temp = vm_file.ReadLine().Trim();
                if (!(temp == ""))
                {
                    if (!(temp.StartsWith("//")))
                    {
                        if (temp.Contains("//"))
                        {
                            temp = temp.Substring(0, temp.IndexOf("//"));
                            temp = temp.Trim();
                        }
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

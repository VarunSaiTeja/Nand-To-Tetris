//function Class2.set 0
(Class2.set)
//push argument 0
@ARG
D=M
@0
A=D+A
D=M
@SP
A=M
M=D
@SP
M=M+1
//pop static 0
@SP
AM=M-1
D=M
@Class2.0
M=D
//push argument 1
@ARG
D=M
@1
A=D+A
D=M
@SP
A=M
M=D
@SP
M=M+1
//pop static 1
@SP
AM=M-1
D=M
@Class2.1
M=D
//push constant 0
@0
D=A
@SP
A=M
M=D
@SP
M=M+1
//return
@LCL
D=M
@R13
M=D
@5
A=D-A
D=M
@R14
M=D
@SP
M=M-1
@SP
A=M
D=M
@ARG
A=M
M=D
@ARG
D=M
@R0
M=D+1
@R13
AMD=M-1
D=M
@THAT
M=D
@R13
AMD=M-1
D=M
@THIS
M=D
@R13
AMD=M-1
D=M
@ARG
M=D
@R13
AMD=M-1
D=M
@LCL
M=D
@R14
A=M
0;JMP
//function Class2.get 0
(Class2.get)
//push static 0
@Class2.0
D=M
@SP
A=M
M=D
@SP
M=M+1
//push static 1
@Class2.1
D=M
@SP
A=M
M=D
@SP
M=M+1
//sub
@SP
AM=M-1
D=M
A=A-1
M=M-D
//return
@LCL
D=M
@R13
M=D
@5
A=D-A
D=M
@R14
M=D
@SP
M=M-1
@SP
A=M
D=M
@ARG
A=M
M=D
@ARG
D=M
@R0
M=D+1
@R13
AMD=M-1
D=M
@THAT
M=D
@R13
AMD=M-1
D=M
@THIS
M=D
@R13
AMD=M-1
D=M
@ARG
M=D
@R13
AMD=M-1
D=M
@LCL
M=D
@R14
A=M
0;JMP
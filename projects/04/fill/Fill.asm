// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

// Put your code here.

//Need to update 32*256=8192 registers to b/w screen
@8192
D=A
//setting max value to count of registers used for screen
@max
M=D

//status variable declaration,0 for white,-1 for black
@status

//newstatus variable declaration and intilization
@newstatus
M=0

//oldstatus variable declaration and intilization
@oldstatus
M=0

//checking whether any key pressed or not
(CHECK)
//updating old status
@newstatus
D=M
@oldstatus
M=D
//checking for new status
@KBD
D=M
@newstatus
M=D
@oldstatus
D=D-M
@CHECK
D;JEQ

(UPDATE)
@newstatus
D=M
//jumping to set status based on KBD value
@BLACK
D;JGT
@WHITE
0;JMP

(BLACK)
@status
//all 16 bits sets to 1
M=-1
@COLOR
0;JMP

(WHITE)
@status
//all 16 bits sets to 0
M=0
@COLOR
0;JMP

(COLOR)
//setting up base screen address in position variable
@SCREEN
D=A
@position
M=D

//intialize i as 0
@i
M=0

(LOOP)
@max
D=M
@i
D=D-M

//after total screen sets to b/w, checks for any key presses in keyboard or not
@CHECK
D;JEQ

@status
D=M
//updating address register to address stored in position register
@position
A=M
//coloring screen b/w for i register in screen memory map
M=D

//incrementing i value
@i
M=M+1
//storing next register address in position
@position
M=M+1
//jumping back to starting of loop
@LOOP
0;JMP
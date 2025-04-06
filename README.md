# SimpleScript Compiler

A learning project to explore and understand fundamental compiler concepts by building a simple programming language
using C# and .NET 8.

## Overview

SimpleScript is a simple yet powerful programming language with a C-like syntax, designed to showcase compiler
construction and language processing concepts. The compiler translates SimpleScript code into C code and then compiles
it to native executables.

## Features

- Custom lexical analyzer and parser
- Type inference system
- Function declarations and invocations
- Control flow statements (IF, WHILE)
- String manipulation
- Input/Output operations
- Native code generation via C compilation

## Example Programs

The following programs can be compiled and executed using the current version of the compiler:

### Fibonacci Number Generator

```
PRINT "How many fibonacci numbers do you want?"
INPUT numsInput
LET nums = ToNumber(numsInput)

IF nums < 0 DO
	PRINT "Number should be greater or equal to zero!"
ENDIF

LET a = 0
LET b = 1
WHILE nums > 0 REPEAT
    PRINT a
    LET c = a + b
    LET a = b
    LET b = c
    LET nums = nums - 1
ENDWHILE
```

### Function Declaration Example

```
FUNC add(int num_1, int num_2) 
BODY
	LET result = num_1 + num_2
	RETURN result
ENDBODY 

PRINT "Result of 23 + 55 is "
PRINT add(23, 55)
```

## Technical Architecture

The compiler is built using a modular architecture with several key components:

1. **Lexical Analyzer (Lexer)**
    - Tokenizes source code
    - Handles keywords, operators, and literals
    - Reference: `SimpleScript.Lexer` project

2. **Parser**
    - Builds Abstract Syntax Tree (AST)
    - Implements recursive descent parsing
    - Reference: `SimpleScript.Parser` project

3. **Code Generator**
    - Translates AST to C code
    - Manages memory allocation
    - Reference: `SimpleScript.Adapter.C` project

4. **Compiler Interface**
    - Orchestrates compilation process
    - Handles file I/O and error reporting
    - Reference: `SimpleScript.Compiler` project

namespace SimpleScript.Lexer
{
    public enum TokenType
    {
        Number, // 12, 24.5
        String, // "Test123"
        Variable, // a, test
        Assign,
        Plus,
        Minus,
        Multiply,
        Divide,
        Power,
        OpenBracket,
        ClosedBracket,
        Smaller,
        Greater,
        SmallerOrEqual,
        GreaterOrEqual,
        Equal,
        Print,
        Input,
        If,
        Endif,
        Let,
        While,
        Repeat,
        EndWhile,
        Func,
        Body,
        EndBody,
        Return,
        IntArg,
        StringArg,
        BoolArg,
        Comma,
        True,
        False,
        NotEqual,
        Do
    }
}
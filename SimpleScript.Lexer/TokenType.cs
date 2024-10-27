namespace SimpleScript.Lexer
{
    public enum TokenType
    {
        Number, // 12, 24.5
        String, // "Test123"
        Variable, // a, test
        ASSIGN,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        POWER,
        OPEN_BRACKET,
        CLOSED_BRACKET,
        SMALLER,
        GREATER,
        SMALLER_OR_EQUAL,
        GREATER_OR_EQUAL,
        EQUAL,
        PRINT,
        INPUT,
        IF,
        ENDIF,
        LET,
        WHILE,
        REPEAT,
        ENDWHILE,
        FUNC,
        BODY,
        ENDBODY,
        RETURN,
        INTARG,
        STRINGARG,
        COMMA,
        TRUE,
        FALSE,
        NOTEQUAL,
        DO
    }
}

using SimpleScriptCompiler.LexicalAnalysis;

public class TokenFactory
{
    public static Token Num(int value)
    {
        return new Token(TokenType.Number, 1, value.ToString());
    }

    public static Token Var(string value)
    {
        return new Token(TokenType.Variable, 1, value);
    }

    public static Token Mul()
    {
        return new Token(TokenType.MULTIPLY, 1);
    }


    public static Token Add()
    {
        return new Token(TokenType.PLUS, 1);
    }


    public static Token Sub()
    {
        return new Token(TokenType.MINUS, 1);
    }

    public static Token Div()
    {
        return new Token(TokenType.DIVIDE, 1);
    }

    public static Token Pow()
    {
        return new Token(TokenType.POWER, 1);
    }
}
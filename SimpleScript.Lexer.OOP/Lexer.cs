namespace SimpleScript.Lexer.OOP;

public class Lexer
{
    public TokenizedProgram Tokenize(string source)
    {
        var result = new TokenizedProgram();
        result.AddTokens([new StringToken("Hello World", 1)]);
        return result;
    }
}
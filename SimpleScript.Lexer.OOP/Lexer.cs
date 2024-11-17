namespace SimpleScript.Lexer.OOP;

public class Lexer
{
    public TokenizedProgram Tokenize(string source)
    {
        
        return new TokenizedProgram([new StringToken("Hallo World", 1)]);
    }
}
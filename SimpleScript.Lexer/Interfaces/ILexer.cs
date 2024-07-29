namespace SimpleScript.Lexer.Interfaces
{
    public interface ILexer
    {
        List<Token> ConvertToTokens(string input, int lineNumber);
    }
}
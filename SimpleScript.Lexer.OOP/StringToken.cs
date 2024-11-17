namespace SimpleScript.Lexer.OOP;

public class StringToken : IToken
{
    public TokenType TokenType => TokenType.String;
    public int Line { get; }
    public string StringValue { get; }

    public StringToken(string stringValue, int line) => (Line, StringValue) = (line, stringValue);
}
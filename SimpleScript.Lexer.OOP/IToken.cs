namespace SimpleScript.Lexer.OOP;

public interface IToken
{
    TokenType TokenType { get; }
    int Line { get; }
}
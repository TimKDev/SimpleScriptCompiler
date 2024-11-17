namespace SimpleScript.Lexer.OOP;

public class TokenizedProgram()
{
    private readonly List<IToken> _tokens = [];

    public IReadOnlyList<IToken> Tokens => _tokens.AsReadOnly();
    public void AddTokens(List<IToken> tokens) => _tokens.AddRange(tokens);
}
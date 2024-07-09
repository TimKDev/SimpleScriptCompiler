using SimpleScript.Lexer;

namespace SimpleScript.Parser
{
    public class Statement
    {
        public List<Token> Tokens { get; set; } = [];

        public int NumberTokens => Tokens.Count;
    }
}

namespace SimpleScriptCompiler.LexicalAnalysis
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string? Value { get; set; }
        public int Line { get; set; }
        public Token(TokenType tokenType, int lineNumber, string? value = null)
        {
            TokenType = tokenType;
            Line = lineNumber;
            Value = value;
        }
    }
}

using EntertainingErrors;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Lexer
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

        public Error CreateError(string message, int? endLine = null)
        {
            string errorMessage = $"Error Line {Line}: {message}";
            if (endLine != null)
            {
                errorMessage = $"Error Lines {Line} - {endLine}: {message}";
            }
            return Error.Create(errorMessage);
        }
    }
}

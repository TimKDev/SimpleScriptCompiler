using EntertainingErrors;

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
            return CreateError(message, Line, endLine);
        }

        public static Error CreateError(string message, int startLine, int? endLine = null)
        {
            string errorMessage = $"Error Line {startLine}: {message}";
            if (endLine != null && startLine != endLine)
            {
                errorMessage = $"Error Lines {startLine} - {endLine}: {message}";
            }
            return Error.Create(errorMessage);
        }
    }
}

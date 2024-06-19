using SimpleScript.Lexer;
using System.Diagnostics.CodeAnalysis;

namespace SimpleScript.Parser.Tests.Helper
{
    internal class TokenComparer : IEqualityComparer<Token>
    {
        public bool Equals(Token? x, Token? y)
        {
            return x?.Line == y?.Line && x?.TokenType == y?.TokenType && x?.Value == y?.Value;
        }

        public int GetHashCode([DisallowNull] Token obj)
        {
            throw new NotImplementedException();
        }
    }
}

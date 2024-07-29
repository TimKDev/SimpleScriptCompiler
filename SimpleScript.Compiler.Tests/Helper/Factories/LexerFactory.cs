using SimpleScript.Lexer.Interfaces;

namespace SimpleScript.Compiler.Tests.Helper.Factories
{
    internal class LexerFactory
    {
        public static ILexer Create()
        {
            return new Lexer.Lexer();
        }
    }
}

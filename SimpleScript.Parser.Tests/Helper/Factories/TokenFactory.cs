using SimpleScript.Lexer;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    public class TokenFactory
    {
        public static Token Num(int value)
        {
            return new Token(TokenType.Number, 1, value.ToString());
        }

        public static Token Str(string value)
        {
            return new Token(TokenType.String, 1, value.ToString());
        }

        public static Token Var(string value)
        {
            return new Token(TokenType.Variable, 1, value);
        }

        public static Token Mul()
        {
            return new Token(TokenType.MULTIPLY, 1);
        }


        public static Token Add()
        {
            return new Token(TokenType.PLUS, 1);
        }


        public static Token Sub()
        {
            return new Token(TokenType.MINUS, 1);
        }

        public static Token Div()
        {
            return new Token(TokenType.DIVIDE, 1);
        }

        public static Token Pow()
        {
            return new Token(TokenType.POWER, 1);
        }

        public static Token Open()
        {
            return new Token(TokenType.OPEN_BRACKET, 1);
        }

        public static Token Close()
        {
            return new Token(TokenType.CLOSED_BRACKET, 1);
        }

        public static Token Print()
        {
            return new Token(TokenType.PRINT, 1);
        }

        public static Token Let()
        {
            return new Token(TokenType.LET, 1);
        }

        public static Token Assign()
        {
            return new Token(TokenType.ASSIGN, 1);
        }

        public static Token Input()
        {
            return new Token(TokenType.INPUT, 1);
        }
    }
}
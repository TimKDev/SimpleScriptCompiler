using SimpleScript.Lexer;

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

        public static Token Func()
        {
            return new Token(TokenType.FUNC, 1);
        }

        public static Token Body()
        {
            return new Token(TokenType.BODY, 1);
        }

        public static Token EndBody()
        {
            return new Token(TokenType.ENDBODY, 1);
        }

        public static Token Return()
        {
            return new Token(TokenType.RETURN, 1);
        }

        public static Token If()
        {
            return new Token(TokenType.IF, 1);
        }

        public static Token EndIf()
        {
            return new Token(TokenType.ENDIF, 1);
        }

        public static Token While()
        {
            return new Token(TokenType.WHILE, 1);
        }

        public static Token EndWhile()
        {
            return new Token(TokenType.ENDWHILE, 1);
        }

        public static Token Int()
        {
            return new Token(TokenType.INT, 1);
        }

        public static Token String()
        {
            return new Token(TokenType.STRING, 1);
        }

        public static Token Comma()
        {
            return new Token(TokenType.COMMA, 1);
        }
    }
}
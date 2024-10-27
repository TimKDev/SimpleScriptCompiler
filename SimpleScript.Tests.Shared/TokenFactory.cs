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

        public static Token Do()
        {
            return new Token(TokenType.DO, 1);
        }

        public static Token EndIf()
        {
            return new Token(TokenType.ENDIF, 1);
        }

        public static Token While()
        {
            return new Token(TokenType.WHILE, 1);
        }

        public static Token Repeat()
        {
            return new Token(TokenType.REPEAT, 1);
        }

        public static Token EndWhile()
        {
            return new Token(TokenType.ENDWHILE, 1);
        }

        public static Token Int()
        {
            return new Token(TokenType.INTARG, 1);
        }

        public static Token StringArg()
        {
            return new Token(TokenType.STRINGARG, 1);
        }

        public static Token Comma()
        {
            return new Token(TokenType.COMMA, 1);
        }

        public static Token True()
        {
            return new Token(TokenType.TRUE, 1);
        }

        public static Token False()
        {
            return new Token(TokenType.FALSE, 1);
        }

        public static Token Equal()
        {
            return new Token(TokenType.EQUAL, 1);
        }

        public static Token NotEqual()
        {
            return new Token(TokenType.NOTEQUAL, 1);
        }

        public static Token Greater()
        {
            return new Token(TokenType.GREATER, 1);
        }

        public static Token Smaller()
        {
            return new Token(TokenType.SMALLER, 1);
        }

        public static Token GreaterOrEqual()
        {
            return new Token(TokenType.GREATER_OR_EQUAL, 1);
        }

        public static Token SmallerOrEqual()
        {
            return new Token(TokenType.SMALLER_OR_EQUAL, 1);
        }
    }
}
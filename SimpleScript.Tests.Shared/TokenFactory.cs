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
            return new Token(TokenType.Multiply, 1);
        }


        public static Token Add()
        {
            return new Token(TokenType.Plus, 1);
        }


        public static Token Sub()
        {
            return new Token(TokenType.Minus, 1);
        }

        public static Token Div()
        {
            return new Token(TokenType.Divide, 1);
        }

        public static Token Pow()
        {
            return new Token(TokenType.Power, 1);
        }

        public static Token Open()
        {
            return new Token(TokenType.OpenBracket, 1);
        }

        public static Token Close()
        {
            return new Token(TokenType.ClosedBracket, 1);
        }

        public static Token Print()
        {
            return new Token(TokenType.Print, 1);
        }

        public static Token Let()
        {
            return new Token(TokenType.Let, 1);
        }

        public static Token Assign()
        {
            return new Token(TokenType.Assign, 1);
        }

        public static Token Input()
        {
            return new Token(TokenType.Input, 1);
        }

        public static Token Func()
        {
            return new Token(TokenType.Func, 1);
        }

        public static Token Body()
        {
            return new Token(TokenType.Body, 1);
        }

        public static Token EndBody()
        {
            return new Token(TokenType.EndBody, 1);
        }

        public static Token Return()
        {
            return new Token(TokenType.Return, 1);
        }

        public static Token If()
        {
            return new Token(TokenType.If, 1);
        }

        public static Token Do()
        {
            return new Token(TokenType.Do, 1);
        }

        public static Token EndIf()
        {
            return new Token(TokenType.Endif, 1);
        }

        public static Token While()
        {
            return new Token(TokenType.While, 1);
        }

        public static Token Repeat()
        {
            return new Token(TokenType.Repeat, 1);
        }

        public static Token EndWhile()
        {
            return new Token(TokenType.EndWhile, 1);
        }

        public static Token Int()
        {
            return new Token(TokenType.IntArg, 1);
        }

        public static Token StringArg()
        {
            return new Token(TokenType.StringArg, 1);
        }

        public static Token BooleanArg()
        {
            return new Token(TokenType.BoolArg, 1);
        }

        public static Token Comma()
        {
            return new Token(TokenType.Comma, 1);
        }

        public static Token True()
        {
            return new Token(TokenType.True, 1);
        }

        public static Token False()
        {
            return new Token(TokenType.False, 1);
        }

        public static Token Equal()
        {
            return new Token(TokenType.Equal, 1);
        }

        public static Token NotEqual()
        {
            return new Token(TokenType.NotEqual, 1);
        }

        public static Token Greater()
        {
            return new Token(TokenType.Greater, 1);
        }

        public static Token Smaller()
        {
            return new Token(TokenType.Smaller, 1);
        }

        public static Token GreaterOrEqual()
        {
            return new Token(TokenType.GreaterOrEqual, 1);
        }

        public static Token SmallerOrEqual()
        {
            return new Token(TokenType.SmallerOrEqual, 1);
        }
    }
}
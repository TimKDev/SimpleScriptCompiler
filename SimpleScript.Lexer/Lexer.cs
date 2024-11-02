using SimpleScript.Lexer.Interfaces;

namespace SimpleScript.Lexer
{
    public class Lexer : ILexer
    {
        private readonly char[] _dividerChars = [' ', '\n', '\t'];
        private readonly char[] _numberChars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'];

        //Order matters in this array otherwise "==" could be translated to "=" "="!
        private readonly string[] _forbiddenVariableNameChars =
            ["==", "<=", ">=", "=", "<", ">", "+", "-", "*", "/", "**", "(", ")", ","];

        private readonly Dictionary<string, TokenType> keywordAndOperatorTokenTypes = new()
        {
            { "LET", TokenType.Let },
            { "PRINT", TokenType.Print },
            { "IF", TokenType.If },
            { "DO", TokenType.Do },
            { "ENDIF", TokenType.Endif },
            { "WHILE", TokenType.While },
            { "REPEAT", TokenType.Repeat },
            { "ENDWHILE", TokenType.EndWhile },
            { "INPUT", TokenType.Input },
            { "FUNC", TokenType.Func },
            { "int", TokenType.IntArg },
            { "string", TokenType.StringArg },
            { "bool", TokenType.BoolArg },
            { ",", TokenType.Comma },
            { "BODY", TokenType.Body },
            { "RETURN", TokenType.Return },
            { "ENDBODY", TokenType.EndBody },
            { "TRUE", TokenType.True },
            { "FALSE", TokenType.False },
            { "=", TokenType.Assign },
            { "==", TokenType.Equal },
            { "!=", TokenType.NotEqual },
            { "<=", TokenType.SmallerOrEqual },
            { ">=", TokenType.GreaterOrEqual },
            { "<", TokenType.Smaller },
            { ">", TokenType.Greater },
            { "+", TokenType.Plus },
            { "-", TokenType.Minus },
            { "*", TokenType.Multiply },
            { "/", TokenType.Divide },
            { "**", TokenType.Power },
            { "(", TokenType.OpenBracket },
            { ")", TokenType.ClosedBracket },
        };

        public List<Token> ConvertToTokens(string input, int lineNumber)
        {
            var untypedTokens = ComputeUntypedTokens(input);
            var tokens = new List<Token>();
            foreach (var untypedToken in untypedTokens)
            {
                if (keywordAndOperatorTokenTypes.TryGetValue(untypedToken.Value, out var operatorTokenType))
                {
                    tokens.Add(new Token(operatorTokenType, lineNumber));
                    continue;
                }

                if (untypedToken.Value.StartsWith("\"") && untypedToken.Value.EndsWith("\""))
                {
                    tokens.Add(new Token(TokenType.String, lineNumber,
                        untypedToken.Value.Substring(1, untypedToken.Value.Length - 2)));
                    continue;
                }

                if (untypedToken.Value.All(v => _numberChars.Contains(v)))
                {
                    tokens.Add(new Token(TokenType.Number, lineNumber, untypedToken.Value));
                    continue;
                }

                tokens.Add(new Token(TokenType.Variable, lineNumber, untypedToken.Value));
            }

            return tokens;
        }

        private List<UntypedToken> ComputeUntypedTokens(string input)
        {
            var untypedTokens = new List<UntypedToken>();
            var i = 0;
            var currentTokenStringValue = string.Empty;
            var currentTokenInStringContext = false;
            while (i < input.Length)
            {
                if (input[i] == '"')
                {
                    currentTokenInStringContext = !currentTokenInStringContext;
                }

                if (!currentTokenInStringContext)
                {
                    var separatorString = _forbiddenVariableNameChars
                        .FirstOrDefault(separatorString => CheckForString(input, i, separatorString));

                    if (separatorString is not null)
                    {
                        if (currentTokenStringValue != string.Empty)
                        {
                            untypedTokens.Add(new UntypedToken(currentTokenStringValue));
                        }

                        currentTokenStringValue = string.Empty;
                        untypedTokens.Add(new UntypedToken(separatorString));
                        i += separatorString.Length;
                        continue;
                    }

                    if (_dividerChars.Contains(input[i]))
                    {
                        if (currentTokenStringValue != string.Empty)
                        {
                            untypedTokens.Add(new UntypedToken(currentTokenStringValue));
                        }

                        currentTokenStringValue = string.Empty;
                        i++;
                        continue;
                    }
                }

                currentTokenStringValue += input[i];
                i++;
            }

            if (currentTokenStringValue != string.Empty)
            {
                untypedTokens.Add(new UntypedToken(currentTokenStringValue));
            }

            return untypedTokens;
        }

        private bool CheckForString(string input, int position, string stringToCheck)
        {
            if (input.Length - position < stringToCheck.Length)
            {
                return false;
            }

            var subStringToCheck = input.Substring(position, stringToCheck.Length);
            return subStringToCheck == stringToCheck;
        }
    }
}
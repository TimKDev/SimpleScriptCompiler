using SimpleScript.Lexer;

namespace SimpleScriptCompiler.LexicalAnalysis
{
    public class Lexer
    {
        private readonly char[] dividerChars = [' ', '\n', '\t'];
        private readonly char[] numberChars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'];
        private readonly string[] forbiddenVariableNameChars = ["=", "==", "<=", ">=", "<", ">", "+", "-", "*", "/", "**", "(", ")"];
        private readonly Dictionary<string, TokenType> keywordAndOperatorTokenTypes = new() {
            { "LET", TokenType.LET },
            { "PRINT", TokenType.PRINT},
            { "IF", TokenType.IF },
            { "ENDIF", TokenType.ENDIF },
            { "WHILE", TokenType.WHILE},
            { "REPEAT", TokenType.REPEAT},
            { "ENDWHILE", TokenType.ENDWHILE },
            { "INPUT", TokenType.INPUT },
            { "=", TokenType.ASSIGN },
            { "==", TokenType.EQUAL },
            { "<=", TokenType.SMALLER_OR_EQUAL },
            { ">=", TokenType.GREATER_OR_EQUAL },
            { "<", TokenType.SMALLER },
            { ">", TokenType.GREATER},
            { "+", TokenType.PLUS },
            { "-", TokenType.MINUS},
            { "*", TokenType.MULTIPLY},
            { "/", TokenType.DIVIDE },
            { "**", TokenType.POWER },
            { "(", TokenType.OPEN_BRACKET },
            { ")", TokenType.CLOSED_BRACKET },
        };
        public List<Token> ConvertToTokens(string input, int lineNumber)
        {
            List<Token> result = new List<Token>();

            int i = 0;
            while (i < input.Length)
            {
                if (CheckForWhiteSpace(input, ref i, lineNumber)) continue;
                if (CheckForKeywordsAndOperators(input, result, ref i, lineNumber)) continue;
                if (CheckForString(input, result, ref i, lineNumber)) continue;
                if (CheckForNumber(input, result, ref i, lineNumber)) continue;
                if (CreateNewVariableToken(input, result, ref i, lineNumber)) continue;
                throw new Exception("Unknown Lexer Error");
            }

            return result;
        }

        private bool CheckForWhiteSpace(string input, ref int i, int lineNumber)
        {
            var currentChar = input[i];
            if (dividerChars.Contains(currentChar))
            {
                i++;
                return true;
            }
            return false;
        }

        private bool CheckForKeywordsAndOperators(string input, List<Token> result, ref int i, int lineNumber)
        {
            foreach (var keywordAndOperatorTokenType in keywordAndOperatorTokenTypes)
            {
                if (CheckIfTheFollowingStringFollows(input, i, keywordAndOperatorTokenType.Key))
                {
                    var newToken = new Token(keywordAndOperatorTokenType.Value, lineNumber);
                    result.Add(newToken);
                    i += keywordAndOperatorTokenType.Key.Length;
                    return true;
                }
            }
            return false;
        }

        private bool CheckForString(string input, List<Token> result, ref int i, int lineNumber)
        {
            var currentChar = input[i];
            if (currentChar == '"')
            {
                var stringTokenValue = GetStringTokenValue(input, i + 1, lineNumber);
                result.Add(new Token(TokenType.String, lineNumber, stringTokenValue));
                i += stringTokenValue.Length + 2; //2 because there are two "" missing in the string value
                return true;
            }
            return false;
        }

        private bool CheckForNumber(string input, List<Token> result, ref int i, int lineNumber)
        {
            var currentChar = input[i];
            if (numberChars.Contains(currentChar))
            {
                var numberTokenValue = GetNumberTokenValue(input, i, lineNumber);
                result.Add(new Token(TokenType.Number, lineNumber, numberTokenValue));
                i += numberTokenValue.Length;
                return true;
            }

            return false;
        }

        private bool CreateNewVariableToken(string input, List<Token> result, ref int i, int lineNumber)
        {
            var variableValue = GetVariableTokenValue(input, i);
            result.Add(new Token(TokenType.Variable, lineNumber, variableValue));
            i += variableValue.Length;
            return true;
        }

        private bool CheckIfTheFollowingStringFollows(string input, int startPositionOfString, string stringToFollow)
        {
            if (input.Length - startPositionOfString < stringToFollow.Length) return false;
            for (int j = 0; j < stringToFollow.Length; j++)
            {
                if (input[startPositionOfString + j] != stringToFollow[j]) return false;
            }
            return true;
        }

        private string GetStringTokenValue(string input, int startPositionOfString, int lineNumber)
        {
            var stringResult = "";
            for (var j = startPositionOfString; j < input.Length; j++)
            {
                var currentChar = input[j];
                if (currentChar == '"')
                {
                    return stringResult;
                }
                stringResult += currentChar;
            }
            throw new Exception($"String not closed in line {lineNumber}! Missing \". ");
        }

        private string GetNumberTokenValue(string input, int startPositionOfNumber, int lineNumber)
        {
            var numberResult = "";
            for (var j = startPositionOfNumber; j < input.Length; j++)
            {
                var currentChar = input[j];
                if (!numberChars.Contains(currentChar))
                {
                    return numberResult;
                }
                numberResult += currentChar;
            }
            return numberResult;
        }

        private string GetVariableTokenValue(string input, int startPositionOfVariable)
        {
            var variableResult = string.Empty;
            for (var j = startPositionOfVariable; j < input.Length; j++)
            {
                var currentChar = input[j];
                if (dividerChars.Contains(currentChar) || forbiddenVariableNameChars.Contains(currentChar.ToString()))
                {
                    return variableResult;
                }
                variableResult += currentChar;
            }
            return variableResult;
        }
    }
}

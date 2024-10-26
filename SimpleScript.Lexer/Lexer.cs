using SimpleScript.Lexer.Interfaces;

namespace SimpleScript.Lexer
{
    public class Lexer : ILexer
    {
        private readonly char[] dividerChars = [' ', '\n', '\t'];
        private readonly char[] numberChars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'];

        private readonly string[] forbiddenVariableNameChars =
            ["=", "==", "<=", ">=", "<", ">", "+", "-", "*", "/", "**", "(", ")", ","];

        private readonly Dictionary<string, TokenType> keywordAndOperatorTokenTypes = new()
        {
            { "LET", TokenType.LET },
            { "PRINT", TokenType.PRINT },
            { "IF", TokenType.IF },
            { "ENDIF", TokenType.ENDIF },
            { "WHILE", TokenType.WHILE },
            { "REPEAT", TokenType.REPEAT },
            { "ENDWHILE", TokenType.ENDWHILE },
            { "INPUT", TokenType.INPUT },
            { "FUNC", TokenType.FUNC },
            { "int", TokenType.INTARG },
            { "string", TokenType.STRINGARG },
            { ",", TokenType.COMMA },
            { "BODY", TokenType.BODY },
            { "RETURN", TokenType.RETURN },
            { "ENDBODY", TokenType.ENDBODY },
            { "=", TokenType.ASSIGN },
            { "==", TokenType.EQUAL },
            { "<=", TokenType.SMALLER_OR_EQUAL },
            { ">=", TokenType.GREATER_OR_EQUAL },
            { "<", TokenType.SMALLER },
            { ">", TokenType.GREATER },
            { "+", TokenType.PLUS },
            { "-", TokenType.MINUS },
            { "*", TokenType.MULTIPLY },
            { "/", TokenType.DIVIDE },
            { "**", TokenType.POWER },
            { "(", TokenType.OPEN_BRACKET },
            { ")", TokenType.CLOSED_BRACKET },
        };

        public List<Token> ConvertToTokens(string input, int lineNumber)
        {
            List<Token> result = [];

            int i = 0;
            while (i < input.Length)
            {
                if (CheckForWhiteSpace(input, ref i, lineNumber))
                {
                    continue;
                }

                if (CheckForKeywordsAndOperators(input, result, ref i, lineNumber))
                {
                    continue;
                }

                if (CheckForString(input, result, ref i, lineNumber))
                {
                    continue;
                }

                if (CheckForNumber(input, result, ref i, lineNumber))
                {
                    continue;
                }

                if (CreateNewVariableToken(input, result, ref i, lineNumber))
                {
                    continue;
                }

                throw new Exception("Unknown Lexer Error");
            }

            return result;
        }

        private bool CheckForWhiteSpace(string input, ref int i, int lineNumber)
        {
            char currentChar = input[i];
            if (dividerChars.Contains(currentChar))
            {
                i++;
                return true;
            }

            return false;
        }

        private bool CheckForKeywordsAndOperators(string input, List<Token> result, ref int i, int lineNumber)
        {
            foreach (KeyValuePair<string, TokenType> keywordAndOperatorTokenType in keywordAndOperatorTokenTypes)
            {
                if (CheckIfTheFollowingStringFollows(input, i, keywordAndOperatorTokenType.Key))
                {
                    var positionAfterNewKeyword = i + keywordAndOperatorTokenType.Key.Length;
                    if (IsKeywordPartOfVariableName(input, positionAfterNewKeyword))
                    {
                        continue;
                    }

                    Token newToken = new(keywordAndOperatorTokenType.Value, lineNumber);
                    result.Add(newToken);
                    i = positionAfterNewKeyword;
                    return true;
                }
            }

            return false;
        }

        private bool IsKeywordPartOfVariableName(string input, int positionAfterNewKeyword)
        {
            if (positionAfterNewKeyword == input.Length - 1) return false;
            var charAfterNewKeyword = input[positionAfterNewKeyword];
            return !forbiddenVariableNameChars.Contains(charAfterNewKeyword.ToString()) ||
                   !dividerChars.Contains(charAfterNewKeyword);
        }

        private bool CheckForString(string input, List<Token> result, ref int i, int lineNumber)
        {
            char currentChar = input[i];
            if (currentChar == '"')
            {
                string stringTokenValue = GetStringTokenValue(input, i + 1, lineNumber);
                result.Add(new Token(TokenType.String, lineNumber, stringTokenValue));
                i += stringTokenValue.Length + 2; //2 because there are two "" missing in the string value
                return true;
            }

            return false;
        }

        private bool CheckForNumber(string input, List<Token> result, ref int i, int lineNumber)
        {
            char currentChar = input[i];
            if (numberChars.Contains(currentChar))
            {
                string numberTokenValue = GetNumberTokenValue(input, i, lineNumber);
                result.Add(new Token(TokenType.Number, lineNumber, numberTokenValue));
                i += numberTokenValue.Length;
                return true;
            }

            return false;
        }

        private bool CreateNewVariableToken(string input, List<Token> result, ref int i, int lineNumber)
        {
            string variableValue = GetVariableTokenValue(input, i);
            result.Add(new Token(TokenType.Variable, lineNumber, variableValue));
            i += variableValue.Length;
            return true;
        }

        private bool CheckIfTheFollowingStringFollows(string input, int startPositionOfString, string stringToFollow)
        {
            if (input.Length - startPositionOfString < stringToFollow.Length)
            {
                return false;
            }

            for (int j = 0; j < stringToFollow.Length; j++)
            {
                if (input[startPositionOfString + j] != stringToFollow[j])
                {
                    return false;
                }
            }

            return true;
        }

        private string GetStringTokenValue(string input, int startPositionOfString, int lineNumber)
        {
            string stringResult = "";
            for (int j = startPositionOfString; j < input.Length; j++)
            {
                char currentChar = input[j];
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
            string numberResult = "";
            for (int j = startPositionOfNumber; j < input.Length; j++)
            {
                char currentChar = input[j];
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
            string variableResult = string.Empty;
            for (int j = startPositionOfVariable; j < input.Length; j++)
            {
                char currentChar = input[j];
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
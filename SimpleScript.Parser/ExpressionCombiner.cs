using EntertainingErrors;
using SimpleScript.Lexer;

namespace SimpleScript.Parser;

public class ExpressionCombiner : IExpressionCombiner
{
    private readonly TokenType[] _tokensToEndExpression =
    [
        TokenType.Let, TokenType.Print, TokenType.Input, TokenType.Func, TokenType.Return, TokenType.If,
        TokenType.While
    ];

    private readonly TokenType[] _binaryOperatorTypes =
    [
        TokenType.Plus,
        TokenType.Multiply,
        TokenType.Equal,
        TokenType.NotEqual,
        TokenType.GreaterOrEqual,
        TokenType.SmallerOrEqual,
        TokenType.Greater,
        TokenType.Smaller,
        //Was ist Assign?????
    ];

    private readonly TokenType[] _bracketTokenTypes =
    [
        TokenType.OpenBracket,
        TokenType.ClosedBracket
    ];

    public Result<List<Token>> GetExpression(List<Token> tokens, int startPosition)
    {
        var result = new List<Token>();
        var openExpression = true;
        for (int i = startPosition; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];

            if (_tokensToEndExpression.Contains(currentToken.TokenType))
            {
                if (openExpression)
                {
                    return Error.Create("Expression is not complete");
                }

                break;
            }

            if (_bracketTokenTypes.Contains(currentToken.TokenType))
            {
                result.Add(currentToken);
                break;
            }

            if (_binaryOperatorTypes.Contains(currentToken.TokenType))
            {
                if (openExpression)
                {
                    return Error.Create("Expression is not complete");
                }

                result.Add(currentToken);
                openExpression = true;
            }

            if (openExpression)
            {
                openExpression = false;

                if (i + 2 < tokens.Count &&
                    (tokens[i].TokenType, tokens[i + 1].TokenType) is (TokenType.Variable, TokenType.OpenBracket))
                {
                    var tokensAfterFirstOpenBracketOfInvocation =
                        GetTokensOfArgumentsOfFunctionInvocation(tokens, i + 2);
                    result.Add(tokens[i]); //Function name
                    result.Add(tokens[i + 1]); // Open Bracket
                    result.AddRange(tokensAfterFirstOpenBracketOfInvocation); // Rest up to closed Bracket. 
                    i += tokensAfterFirstOpenBracketOfInvocation.Count + 1;
                    continue;
                }

                result.Add(currentToken);
                continue;
            }

            result.Add(currentToken);
        }

        return result;
    }

    private static List<Token> GetTokensOfArgumentsOfFunctionInvocation(List<Token> tokens, int startOfExpression)
    {
        var result = new List<Token>();
        var bracketScore = 1;
        var j = startOfExpression;
        while (j < tokens.Count && bracketScore != 0)
        {
            result.Add(tokens[j]);
            if (tokens[j].TokenType == TokenType.OpenBracket)
            {
                bracketScore++;
            }
            else if (tokens[j].TokenType == TokenType.ClosedBracket)
            {
                bracketScore--;
            }

            j++;
        }

        return result;
    }
}
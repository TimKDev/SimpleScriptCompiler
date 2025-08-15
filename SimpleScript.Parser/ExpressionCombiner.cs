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

    public Result<List<Token>> GetExpression(List<Token> tokens, int startPosition)
    {
        var result = new List<Token>();
        for (int i = startPosition; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];
            if (_tokensToEndExpression.Contains(currentToken.TokenType))
            {
                break;
            }

            result.Add(currentToken);
        }

        return result;
    }
}
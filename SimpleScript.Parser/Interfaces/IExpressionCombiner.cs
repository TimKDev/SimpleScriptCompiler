using EntertainingErrors;
using SimpleScript.Lexer;

namespace SimpleScript.Parser;

public interface IExpressionCombiner
{
    Result<List<Token>> GetExpression(List<Token> tokens, int startPosition);
}
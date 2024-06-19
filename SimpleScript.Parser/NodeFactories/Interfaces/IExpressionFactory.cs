using EntertainingErrors;
using SimpleScript.Lexer;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IExpressionFactory
    {
        Result<IExpression> Create(List<Token> inputTokens);
    }
}

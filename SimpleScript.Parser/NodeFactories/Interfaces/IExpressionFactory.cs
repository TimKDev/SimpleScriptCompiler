using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IExpressionFactory
    {
        Result<IExpression> Create(List<Token> inputTokens);
    }
}

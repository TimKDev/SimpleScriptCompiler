using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IExpressionFactory
    {
        Result<IExpression> Create(List<Token> inputTokens);
    }
}

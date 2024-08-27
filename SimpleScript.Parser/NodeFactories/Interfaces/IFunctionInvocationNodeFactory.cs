using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IFunctionInvocationNodeFactory
    {
        Result<FunctionInvocationNode> Create(List<Token> tokens, IExpressionFactory expressionFactory);
    }
}
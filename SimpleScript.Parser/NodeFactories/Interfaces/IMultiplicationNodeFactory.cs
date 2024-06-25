using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IMultiplicationNodeFactory
    {
        Result<MultiplyNode> Create(List<Token> firstOperand, List<Token> secondOperand, IExpressionFactory expressionFactory);
    }
}
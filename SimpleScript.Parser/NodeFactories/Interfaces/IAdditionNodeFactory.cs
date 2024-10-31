using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IAdditionNodeFactory: IBinaryNodeFactory<AddNode>
    {
        Result<AddNode> Create(List<Token> firstOperand, List<Token> secondOperand, IExpressionFactory expressionFactory);
    }
}
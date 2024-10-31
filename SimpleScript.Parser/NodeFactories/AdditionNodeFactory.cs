using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories
{
    public class AdditionNodeFactory : BinaryNodeFactory<AddNode, IAddable>, IAdditionNodeFactory
    {
        protected override Result<AddNode> Factory(IAddable firstOperand, IAddable secondOperant) =>
            AddNode.Create(firstOperand, secondOperant);
    }
}
using EntertainingErrors;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories
{
    public class MultiplicationNodeFactory : BinaryNodeFactory<MultiplyNode, IMultiplyable>, IMultiplicationNodeFactory
    {
        protected override Result<MultiplyNode> Factory(IMultiplyable firstOperand, IMultiplyable secondOperant) =>
            MultiplyNode.Create(firstOperand, secondOperant);
    }
}
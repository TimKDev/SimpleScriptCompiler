using EntertainingErrors;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class SmallerOrEqualNodeFactory : BinaryNodeFactory<SmallerOrEqualNode, ISizeComparable>,
    ISmallerOrEqualNodeFactory
{
    protected override Result<SmallerOrEqualNode> Factory(ISizeComparable firstOperand,
        ISizeComparable secondOperant) =>
        SmallerOrEqualNode.Create(firstOperand, secondOperant);
}
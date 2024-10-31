using EntertainingErrors;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class GreaterOrEqualNodeFactory : BinaryNodeFactory<GreaterOrEqualNode, ISizeComparable>,
    IGreaterOrEqualNodeFactory
{
    protected override Result<GreaterOrEqualNode> Factory(ISizeComparable firstOperand,
        ISizeComparable secondOperant) =>
        GreaterOrEqualNode.Create(firstOperand, secondOperant);
}
using EntertainingErrors;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class SmallerNodeFactory : BinaryNodeFactory<SmallerNode, ISizeComparable>, ISmallerNodeFactory
{
    protected override Result<SmallerNode> Factory(ISizeComparable firstOperand, ISizeComparable secondOperant) =>
        SmallerNode.Create(firstOperand, secondOperant);
}
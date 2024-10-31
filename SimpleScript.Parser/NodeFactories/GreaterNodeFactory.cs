using EntertainingErrors;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class GreaterNodeFactory : BinaryNodeFactory<GreaterNode, ISizeComparable>, IGreaterNodeFactory
{
    protected override Result<GreaterNode> Factory(ISizeComparable firstOperand, ISizeComparable secondOperant) =>
        GreaterNode.Create(firstOperand, secondOperant);
}
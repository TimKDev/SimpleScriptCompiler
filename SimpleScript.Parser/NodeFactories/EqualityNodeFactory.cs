using EntertainingErrors;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class EqualityNodeFactory : BinaryNodeFactory<EqulityNode, IEqualizable>, IEqualityNodeFactory
{
    protected override Result<EqulityNode> Factory(IEqualizable firstOperand, IEqualizable secondOperant) =>
        EqulityNode.Create(firstOperand, secondOperant);
}
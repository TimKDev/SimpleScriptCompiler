using EntertainingErrors;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class EqualityNodeFactory : BinaryNodeFactory<EqualityNode, IEqualizable>, IEqualityNodeFactory
{
    protected override Result<EqualityNode> Factory(IEqualizable firstOperand, IEqualizable secondOperant) =>
        EqualityNode.Create(firstOperand, secondOperant);
}
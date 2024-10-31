using EntertainingErrors;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class InEqualityNodeFactory : BinaryNodeFactory<InEqulityNode, IEqualizable>, IInEqualityNodeFactory
{
    protected override Result<InEqulityNode> Factory(IEqualizable firstOperand, IEqualizable secondOperant) =>
        InEqulityNode.Create(firstOperand, secondOperant);
}
using EntertainingErrors;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class InEqualityNodeFactory : BinaryNodeFactory<InEqualityNode, IEqualizable>, IInEqualityNodeFactory
{
    protected override Result<InEqualityNode> Factory(IEqualizable firstOperand, IEqualizable secondOperant) =>
        InEqualityNode.Create(firstOperand, secondOperant);
}
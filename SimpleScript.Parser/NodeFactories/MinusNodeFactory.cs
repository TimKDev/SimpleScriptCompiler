using EntertainingErrors;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories;

public class MinusNodeFactory : BinaryNodeFactory<MinusNode, IAddable>, IMinusNodeFactory
{
    protected override Result<MinusNode> Factory(IAddable firstOperand, IAddable secondOperant) =>
        MinusNode.Create(firstOperand, secondOperant);
}
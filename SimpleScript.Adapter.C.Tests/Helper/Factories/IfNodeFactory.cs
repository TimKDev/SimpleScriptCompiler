using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories;

internal static class IfNodeFactory
{
    public static IfNode Create(IExpression conditionNode, List<IBodyNode> ifBodyNodes, int startLine, int endLine)
    {
        var bodyNode = BodyNodeFactory.Create(ifBodyNodes, startLine, endLine);
        return IfNode.Create(conditionNode, bodyNode).Value;
    }
}
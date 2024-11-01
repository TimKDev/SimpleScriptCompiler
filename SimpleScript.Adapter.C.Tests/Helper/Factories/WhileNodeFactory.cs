using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories;

internal static class WhileNodeFactory
{
    public static WhileNode Create(IExpression conditionNode, List<IBodyNode> whileBodyNodes, int startLine,
        int endLine)
    {
        var bodyNode = BodyNodeFactory.Create(whileBodyNodes, startLine, endLine);
        return WhileNode.Create(conditionNode, bodyNode).Value;
    }
}
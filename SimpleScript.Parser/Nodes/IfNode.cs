using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes;

public class IfNode : NodeBase, IBodyNode
{
    public IExpression Condition { get; }
    public BodyNode Body { get; }

    private IfNode(IExpression condition, BodyNode body, int startLine, int endLine) : base(startLine, endLine)
    {
        Condition = condition;
        Body = body;
    }

    public static Result<IfNode> Create(IExpression condition, BodyNode bodyNode)
    {
        int startLine = condition.StartLine;
        int endLine = bodyNode.EndLine;

        return new IfNode(condition, bodyNode, startLine, endLine);
    }
}
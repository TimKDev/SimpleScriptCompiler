using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes;

public class WhileNode : NodeBase, IBodyNode
{
    public IExpression Condition { get; }
    public BodyNode Body { get; }

    private WhileNode(IExpression condition, BodyNode body, int startLine, int endLine) : base(startLine, endLine)
    {
        Condition = condition;
        Body = body;
    }

    public static Result<WhileNode> Create(IExpression condition, BodyNode body)
    {
        int startLine = condition.StartLine;
        int endLine = body.EndLine;

        return new WhileNode(condition, body, startLine, endLine);
    }
}
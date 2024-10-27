using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes;

public class GreaterNode : NodeBase, IBinaryOperation<ISizeComparable>
{
    public ISizeComparable FirstArgument { get; }
    public ISizeComparable SecondArgument { get; }

    private GreaterNode(ISizeComparable firstArgument, ISizeComparable secondArgument, int startLine, int endLine) :
        base(startLine, endLine)
    {
        FirstArgument = firstArgument;
        SecondArgument = secondArgument;
    }

    public static Result<GreaterNode> Create(ISizeComparable firstArgument, ISizeComparable secondArgument)
    {
        int startLine = firstArgument.StartLine;
        int endLine = secondArgument.EndLine;

        return new GreaterNode(firstArgument, secondArgument, startLine, endLine);
    }
}
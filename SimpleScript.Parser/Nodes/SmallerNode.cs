using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes;

public class SmallerNode : NodeBase, IBinaryOperation<ISizeComparable>
{
    public ISizeComparable FirstArgument { get; }
    public ISizeComparable SecondArgument { get; }

    private SmallerNode(ISizeComparable firstArgument, ISizeComparable secondArgument, int startLine, int endLine) :
        base(startLine, endLine)
    {
        FirstArgument = firstArgument;
        SecondArgument = secondArgument;
    }

    public static Result<SmallerNode> Create(ISizeComparable firstArgument, ISizeComparable secondArgument)
    {
        int startLine = firstArgument.StartLine;
        int endLine = secondArgument.EndLine;

        return new SmallerNode(firstArgument, secondArgument, startLine, endLine);
    }
}
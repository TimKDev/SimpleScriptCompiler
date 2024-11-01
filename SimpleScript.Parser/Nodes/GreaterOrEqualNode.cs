using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes;

public class GreaterOrEqualNode : NodeBase, IBinaryOperation<ISizeComparable>, IEqualizable
{
    public ISizeComparable FirstArgument { get; }
    public ISizeComparable SecondArgument { get; }

    private GreaterOrEqualNode(ISizeComparable firstArgument, ISizeComparable secondArgument, int startLine,
        int endLine) :
        base(startLine, endLine)
    {
        FirstArgument = firstArgument;
        SecondArgument = secondArgument;
    }

    public static Result<GreaterOrEqualNode> Create(ISizeComparable firstArgument, ISizeComparable secondArgument)
    {
        int startLine = firstArgument.StartLine;
        int endLine = secondArgument.EndLine;

        return new GreaterOrEqualNode(firstArgument, secondArgument, startLine, endLine);
    }
}
using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes;

public class InEqualityNode : NodeBase, IBinaryOperation<IEqualizable>, IEqualizable
{
    public IEqualizable FirstArgument { get; private set; }
    public IEqualizable SecondArgument { get; private set; }

    private InEqualityNode(IEqualizable firstArgument, IEqualizable secondArgument, int startLine, int endLine) : base(
        startLine, endLine)
    {
        FirstArgument = firstArgument;
        SecondArgument = secondArgument;
    }

    public static Result<InEqualityNode> Create(IEqualizable firstArgument, IEqualizable secondArgument)
    {
        int startLine = firstArgument.StartLine;
        int endLine = secondArgument.EndLine;
        string? staticTypeCheckingErrorMessage = StaticTypesCompatible(firstArgument, secondArgument);
        if (staticTypeCheckingErrorMessage is not null)
        {
            return CreateError(staticTypeCheckingErrorMessage, startLine, endLine);
        }

        return new InEqualityNode(firstArgument, secondArgument, startLine, endLine);
    }

    private static string? StaticTypesCompatible(IEqualizable firstArgument, IEqualizable secondArgument) =>
        (firstArgument, secondArgument) switch
        {
            (NumberNode, StringNode) => GetStaticTypError(NumberNode.TypeName, StringNode.TypeName),
            (StringNode, NumberNode) => GetStaticTypError(StringNode.TypeName, NumberNode.TypeName),
            _ => null,
        };

    private static string GetStaticTypError(ValueTypes type1, ValueTypes type2) =>
        $"Inequality comparison between types {type1} and {type2} is not allowed.";
}
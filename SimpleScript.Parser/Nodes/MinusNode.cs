using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes;

public class MinusNode : NodeBase, IBinaryOperation<IAddable>, IAddable, IMultiplyable, ISizeComparable
{
    public IAddable FirstArgument { get; private set; }
    public IAddable SecondArgument { get; private set; }

    private MinusNode(IAddable firstArgument, IAddable secondArgument, int startLine, int endLine) : base(startLine,
        endLine)
    {
        FirstArgument = firstArgument;
        SecondArgument = secondArgument;
    }

    public static Result<MinusNode> Create(IAddable firstArgument, IAddable secondArgument)
    {
        int startLine = firstArgument.StartLine;
        int endLine = secondArgument.EndLine;

        string? staticTypeCheckingErrorMessage = StaticTypesCompatible(firstArgument, secondArgument);
        if (staticTypeCheckingErrorMessage is not null)
        {
            return CreateError(staticTypeCheckingErrorMessage, startLine, endLine);
        }

        return new MinusNode(firstArgument, secondArgument, startLine, endLine);
    }

    private static string? StaticTypesCompatible(IAddable firstArgument, IAddable secondArgument) =>
        (firstArgument, secondArgument) switch
        {
            (NumberNode, StringNode) => GetStaticTypError(NumberNode.TypeName, StringNode.TypeName),
            (StringNode, NumberNode) => GetStaticTypError(StringNode.TypeName, NumberNode.TypeName),
            _ => null,
        };

    private static string GetStaticTypError(ValueTypes type1, ValueTypes type2) =>
        $"Subtraction between types {type1} and {type2} is not allowed.";
}
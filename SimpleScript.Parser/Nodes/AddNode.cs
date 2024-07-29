using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class AddNode : NodeBase, IBinaryOperation<IAddable>, IPrintableNode, IAddable, IMultiplyable
    {
        public IAddable FirstArgument { get; private set; }
        public IAddable SecondArgument { get; private set; }

        private AddNode(IAddable firstArgument, IAddable secondArgument, int startLine, int endLine) : base(startLine, endLine)
        {
            FirstArgument = firstArgument;
            SecondArgument = secondArgument;
        }

        public

        public static Result<AddNode> Create(IAddable firstArgument, IAddable secondArgument)
        {
            int startLine = firstArgument.StartLine;
            int endLine = secondArgument.EndLine;

            string? staticTypeCheckingErrorMessage = StaticTypesCompatible(firstArgument, secondArgument);
            if (staticTypeCheckingErrorMessage is not null)
            {
                return CreateError(staticTypeCheckingErrorMessage, startLine, endLine);
            }

            return new AddNode(firstArgument, secondArgument, startLine, endLine);
        }

        private static string? StaticTypesCompatible(IAddable firstArgument, IAddable secondArgument) => (firstArgument, secondArgument) switch
        {
            (NumberNode, StringNode) => GetStaticTypError(NumberNode.TypeName, StringNode.TypeName),
            (StringNode, NumberNode) => GetStaticTypError(StringNode.TypeName, NumberNode.TypeName),
            _ => null,
        };

        private static string GetStaticTypError(string type1, string type2) => $"Addition between types {type1} and {type2} is not allowed.";
    }
}
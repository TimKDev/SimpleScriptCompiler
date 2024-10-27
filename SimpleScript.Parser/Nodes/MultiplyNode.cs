using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class MultiplyNode : NodeBase, IBinaryOperation<IMultiplyable>, IAddable, IMultiplyable
    {
        public IMultiplyable FirstArgument { get; }
        public IMultiplyable SecondArgument { get; }

        private MultiplyNode(IMultiplyable firstArgument, IMultiplyable secondArgument, int startLine, int endLine) :
            base(startLine, endLine)
        {
            FirstArgument = firstArgument;
            SecondArgument = secondArgument;
        }

        public static Result<MultiplyNode> Create(IMultiplyable firstArgument, IMultiplyable secondArgument)
        {
            int startLine = firstArgument.StartLine;
            int endLine = secondArgument.EndLine;

            return new MultiplyNode(firstArgument, secondArgument, startLine, endLine);
        }
    }
}
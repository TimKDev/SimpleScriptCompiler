using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class MultiplyNode : NodeBase, IBinaryOperation<IMultiplyable>, IPrintableNode, IAddable, IMultiplyable
    {
        public IMultiplyable FirstArgument { get; private set; }
        public IMultiplyable SecondArgument { get; private set; }

        private MultiplyNode(IMultiplyable firstArgument, IMultiplyable secondArgument, int startLine, int endLine) : base(startLine, endLine)
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
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class MultiplyNode : IBinaryOperation<IMultiplyable>, IExpression
    {
        public IMultiplyable FirstArgument { get; private set; }
        public IMultiplyable SecondArgument { get; private set; }

        public MultiplyNode(IMultiplyable firstArgument, IMultiplyable secondArgument)
        {
            FirstArgument = firstArgument;
            SecondArgument = secondArgument;
        }
    }
}
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class AddNode : IBinaryOperation<IAddable>, IExpression
    {
        public IAddable FirstArgument { get; private set; }
        public IAddable SecondArgument { get; private set; }

        public AddNode(IAddable firstArgument, IAddable secondArgument)
        {
            FirstArgument = firstArgument;
            SecondArgument = secondArgument;
        }
    }
}
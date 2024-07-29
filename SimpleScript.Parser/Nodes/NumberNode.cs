using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class NumberNode : NodeBase, IExpression, IPrintableNode, IAddable, IMultiplyable
    {
        public static ValueTypes TypeName = ValueTypes.Number;
        public int Value { get; private set; }

        public NumberNode(int value, int startLine, int endLine) : base(startLine, endLine)
        {
            Value = value;
        }
    }
}

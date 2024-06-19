using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class NumberNode : IPrintableNode, IAddable, IMultiplyable
    {
        public int Value { get; set; }

        public NumberNode(int value)
        {
            Value = value;
        }
    }
}

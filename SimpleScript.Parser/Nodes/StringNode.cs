using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class StringNode : IPrintableNode, IAddable
    {
        public string Value { get; set; }

        public StringNode(string value)
        {
            Value = value;
        }
    }
}

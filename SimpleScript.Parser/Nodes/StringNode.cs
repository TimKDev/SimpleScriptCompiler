using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class StringNode : NodeBase, IAddable, IEqualizable
    {
        public static ValueTypes TypeName = ValueTypes.String;
        public string Value { get; private set; }

        public StringNode(string value, int startLine, int endLine) : base(startLine, endLine)
        {
            Value = value;
        }
    }
}
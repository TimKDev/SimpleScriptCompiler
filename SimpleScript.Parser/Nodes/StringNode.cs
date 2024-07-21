using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class StringNode : NodeBase, IExpression, IPrintableNode, IAddable
    {
        public static string TypeName = "String";
        public string Value { get; private set; }

        public StringNode(string value, int startLine, int endLine) : base(startLine, endLine)
        {
            Value = value;
        }
    }
}

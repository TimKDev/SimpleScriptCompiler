using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class VariableNode : NodeBase, IExpression, IMultiplyable, IPrintableNode, IAddable
    {
        public string Name { get; private set; }
        public ValueTypes Type { get; init; }
        public VariableNode(string name, int startLine, int endLine) : base(startLine, endLine)
        {
            Name = name;
        }
    }
}
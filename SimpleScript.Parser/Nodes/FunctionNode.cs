using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class FunctionNode : NodeBase, IBodyNode
    {
        public string Name { get; }
        public List<FunctionArgumentNode> Arguments { get; }
        public BodyNode Body { get; }
        public FunctionNode(string name, List<FunctionArgumentNode> arguments, BodyNode body, int startLine, int endLine) : base(startLine, endLine)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
        }
    }
}

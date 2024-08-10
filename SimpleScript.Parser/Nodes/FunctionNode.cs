using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class FunctionNode : NodeBase, IProgramRootNodes
    {
        public string Name { get; }
        public List<FunctionArgumentNode> Arguments { get; }
        public FunctionNode(string name, List<FunctionArgumentNode> arguments, int startLine, int endLine) : base(startLine, endLine)
        {
            Name = name;
            Arguments = arguments;
        }
    }
}

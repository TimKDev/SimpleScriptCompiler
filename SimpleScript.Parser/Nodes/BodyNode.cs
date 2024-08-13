using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class BodyNode : NodeBase
    {
        public List<IBodyNode> ChildNodes { get; }
        public BodyNode(List<IBodyNode> childNodes, int startLine, int endLine) : base(startLine, endLine)
        {
            ChildNodes = childNodes;
        }
    }
}
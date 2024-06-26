using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class PrintNode : IProgramRootNodes
    {
        public List<IPrintableNode> ChildNodes = [];
    }
}

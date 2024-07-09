using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class PrintNode : IProgramRootNodes
    {
        public IPrintableNode NodeToPrint { get; private set; }
        public PrintNode(IPrintableNode printableNode)
        {
            NodeToPrint = printableNode;
        }
    }
}

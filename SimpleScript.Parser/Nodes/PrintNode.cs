using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class PrintNode : IBodyNode
    {
        public IPrintableNode NodeToPrint { get; private set; }
        private PrintNode(IPrintableNode printableNode)
        {
            NodeToPrint = printableNode;
        }

        public static Result<PrintNode> Create(IPrintableNode printableNode)
        {
            return new PrintNode(printableNode);
        }
    }
}

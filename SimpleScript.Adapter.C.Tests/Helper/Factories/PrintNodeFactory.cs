using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class PrintNodeFactory
    {
        public static PrintNode Create(IPrintableNode printableNode)
        {
            return ErrorHelper.AssertResultSuccess(PrintNode.Create(printableNode));
        }
    }
}

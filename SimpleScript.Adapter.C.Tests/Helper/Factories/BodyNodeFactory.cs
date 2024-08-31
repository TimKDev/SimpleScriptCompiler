using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class BodyNodeFactory   
    {
        public static BodyNode Create(List<IBodyNode> childNodes, int startLine, int endLine)
        {
            return new BodyNode(childNodes, startLine, endLine);
        }
    }
}

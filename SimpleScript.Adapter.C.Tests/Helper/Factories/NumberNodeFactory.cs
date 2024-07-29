using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal class NumberNodeFactory
    {
        public static NumberNode Create(int number, int startLine, int endLine)
        {
            return new NumberNode(number, startLine, endLine);
        }
    }
}

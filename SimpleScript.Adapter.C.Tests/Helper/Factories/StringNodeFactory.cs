using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal class StringNodeFactory
    {
        public static StringNode Create(string value, int startLine, int endLine)
        {
            return new StringNode(value, startLine, endLine);
        }
    }
}

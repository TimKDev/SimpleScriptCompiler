using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal class VariableNodeFactory
    {
        public static VariableNode Create(string name, int startLine, int endLine)
        {
            return new VariableNode(name, startLine, endLine);
        }
    }
}

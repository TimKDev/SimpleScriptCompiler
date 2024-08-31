using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class FunctionNodeFactory
    {
        public static FunctionNode Create(string name, List<FunctionArgumentNode> arguments, BodyNode body, int startLine, int endLine)
        {
            return new FunctionNode(name, arguments, body, startLine, endLine);
        }
    }
}

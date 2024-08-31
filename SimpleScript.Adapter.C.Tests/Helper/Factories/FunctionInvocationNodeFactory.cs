using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class FunctionInvocationNodeFactory
    {
        public static FunctionInvocationNode Create(string functionName, IExpression[] functionArguments, int startLine, int endLine)
        {
            return new FunctionInvocationNode(functionName, functionArguments, startLine, endLine);
        }
    }
}

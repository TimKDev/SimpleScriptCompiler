using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class FunctionArgumentFactory
    {
        public static FunctionArgumentNode Create(ArgumentType argumentType, string argumentName, int startLine, int endLine)
        {
            return new FunctionArgumentNode(argumentType, argumentName, startLine, endLine);
        }
    }
}

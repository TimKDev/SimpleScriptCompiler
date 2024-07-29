using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal class MultiplyNodeFactory
    {
        public static MultiplyNode Create(IMultiplyable firstArgument, IMultiplyable secondArgument)
        {
            return ErrorHelper.AssertResultSuccess(MultiplyNode.Create(firstArgument, secondArgument));
        }
    }
}

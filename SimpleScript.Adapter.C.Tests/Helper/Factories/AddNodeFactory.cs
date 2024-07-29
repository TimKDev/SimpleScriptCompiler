using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class AddNodeFactory
    {
        public static AddNode Create(IAddable firstArgument, IAddable secondArgument)
        {
            return ErrorHelper.AssertResultSuccess(AddNode.Create(firstArgument, secondArgument));
        }
    }
}

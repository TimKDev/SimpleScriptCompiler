using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class ReturnNodeFactory
    {
        public static ReturnNode Create(IExpression nodeToReturn)
        {
            return ReturnNode.Create(nodeToReturn).Value;
        }
    }
}

using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.NodeFactories.Interfaces;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    internal static class ReturnNodeFactoryFactory
    {
        public static IReturnNodeFactory Create()
        {
            return new ReturnNodeFactory(ExpressionFactoryFactory.Create());
        }
    }
}

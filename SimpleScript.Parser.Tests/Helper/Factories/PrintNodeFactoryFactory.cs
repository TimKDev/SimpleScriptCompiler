using SimpleScript.Parser.NodeFactories;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    internal class PrintNodeFactoryFactory
    {
        public static PrintNodeFactory Create()
        {
            return new PrintNodeFactory(ExpressionFactoryFactory.Create());
        }
    }
}

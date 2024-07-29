using SimpleScript.Parser.NodeFactories;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    public class PrintNodeFactoryFactory
    {
        public static PrintNodeFactory Create()
        {
            return new PrintNodeFactory(ExpressionFactoryFactory.Create());
        }
    }
}

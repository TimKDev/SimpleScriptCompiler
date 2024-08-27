using SimpleScript.Parser.NodeFactories;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    internal static class ExpressionFactoryFactory
    {
        public static ExpressionFactory Create()
        {
            return new ExpressionFactory(new AdditionNodeFactory(), new MultiplicationNodeFactory(), FunctionInvocationNodeFactoryFactory.Create());
        }
    }
}
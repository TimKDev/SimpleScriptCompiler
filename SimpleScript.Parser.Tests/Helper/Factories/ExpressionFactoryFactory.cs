using SimpleScript.Parser.NodeFactories;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    internal static class ExpressionFactoryFactory
    {
        public static ExpressionFactory Create()
        {
            return new ExpressionFactory(new AdditionNodeFactory(), new MultiplicationNodeFactory(),
                FunctionInvocationNodeFactoryFactory.Create(), new EqualityNodeFactory(), new InEqualityNodeFactory(),
                new GreaterNodeFactory(), new GreaterOrEqualNodeFactory(), new SmallerNodeFactory(),
                new SmallerOrEqualNodeFactory(), new MinusNodeFactory());
        }
    }
}
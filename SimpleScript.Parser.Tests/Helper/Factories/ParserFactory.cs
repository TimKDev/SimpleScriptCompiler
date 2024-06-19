using SimpleScript.Parser.NodeFactories;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    internal static class ParserFactory
    {
        public static Parser Create()
        {
            ExpressionFactory expressionFactory = ExpressionFactoryFactory.Create();

            return new Parser(expressionFactory);
        }
    }
}

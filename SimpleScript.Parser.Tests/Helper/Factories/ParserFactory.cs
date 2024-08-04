namespace SimpleScript.Parser.Tests.Helper.Factories
{
    internal static class ParserFactory
    {
        public static Parser Create()
        {
            return new Parser(StatementCombinerFactory.Create(), VariableAssignmentNodeFactoryFactory.Create(), PrintNodeFactoryFactory.Create(), InputNodeFactoryFactory.Create());
        }
    }
}

using SimpleScript.Parser.NodeFactories;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    internal static class VariableAssignmentNodeFactoryFactory
    {
        public static VariableDeclarationNodeFactory Create()
        {
            return new VariableDeclarationNodeFactory(ExpressionFactoryFactory.Create());
        }
    }
}

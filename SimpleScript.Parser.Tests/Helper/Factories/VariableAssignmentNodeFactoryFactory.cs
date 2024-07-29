using SimpleScript.Parser.NodeFactories;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    public static class VariableAssignmentNodeFactoryFactory
    {
        public static VariableDeclarationNodeFactory Create()
        {
            return new VariableDeclarationNodeFactory(ExpressionFactoryFactory.Create());
        }
    }
}

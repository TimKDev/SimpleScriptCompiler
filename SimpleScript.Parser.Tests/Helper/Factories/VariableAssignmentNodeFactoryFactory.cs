using SimpleScript.Parser.NodeFactories;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    internal static class VariableAssignmentNodeFactoryFactory
    {
        public static VariableAssignmentNodeFactory Create()
        {
            return new VariableAssignmentNodeFactory(ExpressionFactoryFactory.Create());
        }
    }
}

using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Tests.Helper.Factories;

public static class BodyNodeFactoryFactory
{
    public static IBodyNodeFactory Create()
    {
        return new BodyNodeFactory(StatementCombinerFactory.Create(), VariableAssignmentNodeFactoryFactory.Create(), PrintNodeFactoryFactory.Create(), InputNodeFactoryFactory.Create(), FunctionNodeFactoryFactory.Create(), ReturnNodeFactoryFactory.Create());
    }
}

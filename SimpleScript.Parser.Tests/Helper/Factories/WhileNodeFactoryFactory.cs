using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Tests.Helper.Factories;

public static class WhileNodeFactoryFactory
{
    public static IWhileNodeFactory Create() => new WhileNodeFactory(ExpressionFactoryFactory.Create());
}
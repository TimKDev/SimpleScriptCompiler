using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Tests.Helper.Factories;

public static class IfNodeFactoryFactory
{
    public static IIfNodeFactory Create() => new IfNodeFactory(ExpressionFactoryFactory.Create());
}
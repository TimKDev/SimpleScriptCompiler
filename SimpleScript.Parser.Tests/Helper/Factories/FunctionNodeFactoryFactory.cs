using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.NodeFactories;

namespace SimpleScript.Parser.Tests.Helper.Factories
{
    internal static class FunctionNodeFactoryFactory
    {
        public static IFunctionNodeFactory Create()
        {
            return new FunctionNodeFactory();
        }
    }
}

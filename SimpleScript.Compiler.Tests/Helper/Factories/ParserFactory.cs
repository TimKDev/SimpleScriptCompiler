using SimpleScript.Parser.Interfaces;

namespace SimpleScript.Compiler.Tests.Helper.Factories
{
    internal class ParserFactory
    {
        public static IParser Create()
        {
            return new Parser.Parser(BodyNodeFactoryFactory.Create());
        }
    }
}

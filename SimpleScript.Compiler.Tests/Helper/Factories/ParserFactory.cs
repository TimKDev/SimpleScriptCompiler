using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.Tests.Helper.Factories;

namespace SimpleScript.Compiler.Tests.Helper.Factories
{
    internal class ParserFactory
    {
        public static IParser Create()
        {
            return new Parser.Parser(StatementCombinerFactory.Create(), VariableAssignmentNodeFactoryFactory.Create(), PrintNodeFactoryFactory.Create(), InputNodeFactoryFactory.Create());
        }
    }
}

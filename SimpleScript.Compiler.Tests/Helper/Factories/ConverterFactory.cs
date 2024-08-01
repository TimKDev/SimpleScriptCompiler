using SimpleScript.Adapter.Abstractions;
using SimpleScript.Adapter.C;

namespace SimpleScript.Compiler.Tests.Helper.Factories
{
    internal class ConverterFactory
    {
        public static IConverter Create()
        {
            return new ProgramConverterToC();
        }
    }
}

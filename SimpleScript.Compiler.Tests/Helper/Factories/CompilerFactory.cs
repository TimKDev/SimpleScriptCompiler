using SimpleScript.Adapter.Abstractions;
using SimpleScript.Adapter.C;

namespace SimpleScript.Compiler.Tests.Helper.Factories
{
    internal class CompilerFactory
    {
        public static ICompiler Create()
        {
            return new CompileCCode();
        }
    }
}

using SimpleScript.Adapter.C;
using SimpleScript.Converter.C;

namespace SimpleScript.Compiler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConverterToCCode converter = new();
            string cCode = converter.ConvertToCCode(null);
            CompileCCode compiler = new();
            compiler.CompileAndExecute(cCode);
        }
    }
}

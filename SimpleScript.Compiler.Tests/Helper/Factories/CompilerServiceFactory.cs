using SimpleScript.Compiler.Services;

namespace SimpleScript.Compiler.Tests.Helper.Factories;

internal static class CompilerServiceFactory
{
    public static ICompileService Create() => new CompileService(LexerFactory.Create(),
        ParserFactory.Create(), ConverterFactory.Create(), CompilerFactory.Create());
}
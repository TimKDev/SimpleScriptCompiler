using ConsoleCore;
using ConsoleCore.Extensions;
using ConsoleCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleScript.Adapter.C.Extensions;
using SimpleScript.Compiler.Command;
using SimpleScript.Compiler.Extensions;
using SimpleScript.Compiler.Services;
using SimpleScript.Lexer.Extensions;
using SimpleScript.Parser.Extensions;

namespace SimpleScript.Compiler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                    .RegisterCAdapterService()
                    .RegisterLexer()
                    .RegisterParser()
                    .AddCompilerBase()
                    .AddLogging(builder =>
                    {
                        builder
                            .AddConsole()
                            .SetMinimumLevel(LogLevel.Debug);
                    })
                    .BuildServiceProvider();

            serviceProvider.StartConsoleApplication(args);
        }
    }
}

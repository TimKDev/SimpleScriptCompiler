using ConsoleCore;
using ConsoleCore.Extensions;
using ConsoleCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SimpleScript.Adapter.C.Extensions;
using SimpleScript.Compiler.Command;
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
                    .AddSingleton<IConsoleBase, ConsoleBase>()
                    .AddSingleton<IConsoleCommand, ExecuteCommand>()
                    .BuildServiceProvider();

            serviceProvider.StartConsoleApplication(args);
        }
    }
}

using ConsoleCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleScript.Adapter.C.Extensions;
using SimpleScript.Compiler.Extensions;
using SimpleScript.Lexer.Extensions;
using SimpleScript.Parser.Extensions;

namespace SimpleScript.Compiler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ServiceProvider serviceProvider = new ServiceCollection()
                .Configure<CompilerSettings>(configuration.GetSection(CompilerSettings.SectionName))
                .RegisterCAdapterService()
                .RegisterLexer()
                .RegisterParser()
                .AddCompilerBase()
                .AddLogging(builder =>
                {
                    builder
                        .SetMinimumLevel(LogLevel.Debug);
                })
                .BuildServiceProvider();

            serviceProvider.StartConsoleApplication(args);
        }
    }
}
using ConsoleCore;
using ConsoleCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SimpleScript.Compiler.Command;
using SimpleScript.Compiler.Services;

namespace SimpleScript.Compiler.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCompilerBase(this IServiceCollection services)
    {
        services.AddScoped<ICompileService, CompileService>()
            .AddSingleton<IConsoleBase, ConsoleBase>()
            .AddSingleton<IConsoleCommand, ExecuteCommand>()
            .AddSingleton<IConsoleCommand, CompileCommand>();
        
        return services;
    }
}
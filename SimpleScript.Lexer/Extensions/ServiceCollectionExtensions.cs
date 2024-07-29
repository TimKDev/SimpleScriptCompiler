using Microsoft.Extensions.DependencyInjection;
using SimpleScript.Lexer.Interfaces;

namespace SimpleScript.Lexer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterLexer(this IServiceCollection services)
        {
            services.AddTransient<ILexer, Lexer>();

            return services;
        }
    }
}

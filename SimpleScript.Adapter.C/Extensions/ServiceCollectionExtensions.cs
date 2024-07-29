using Microsoft.Extensions.DependencyInjection;
using SimpleScript.Adapter.Abstractions;

namespace SimpleScript.Adapter.C.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterCAdapterService(this IServiceCollection services)
        {
            services.AddTransient<ICompiler, CompileCCode>();
            services.AddTransient<IConverter, ConverterToCCode>();

            return services;
        }
    }
}

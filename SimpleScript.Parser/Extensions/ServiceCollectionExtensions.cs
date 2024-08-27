using Microsoft.Extensions.DependencyInjection;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.NodeFactories.Interfaces;

namespace SimpleScript.Parser.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterParser(this IServiceCollection services)
        {
            services.AddTransient<IParser, Parser>();
            services.AddTransient<IStatementCombiner, StatementCombiner>();
            services.AddTransient<IAdditionNodeFactory, AdditionNodeFactory>();
            services.AddTransient<IMultiplicationNodeFactory, MultiplicationNodeFactory>();
            services.AddTransient<IExpressionFactory, ExpressionFactory>();
            services.AddTransient<IPrintNodeFactory, PrintNodeFactory>();
            services.AddTransient<IVariableDeclarartionNodeFactory, VariableDeclarationNodeFactory>();
            services.AddTransient<IInputNodeFactory, InputNodeFactory>();
            services.AddTransient<IFunctionNodeFactory, FunctionNodeFactory>();
            services.AddTransient<IBodyNodeFactory, BodyNodeFactory>();
            services.AddTransient<IReturnNodeFactory, ReturnNodeFactory>();
            services.AddTransient<IFunctionInvocationNodeFactory, FunctionInvocationNodeFactory>();

            return services;
        }
    }
}

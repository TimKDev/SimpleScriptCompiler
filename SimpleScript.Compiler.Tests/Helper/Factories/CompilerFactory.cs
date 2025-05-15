using Microsoft.Extensions.Logging;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Adapter.C;

namespace SimpleScript.Compiler.Tests.Helper.Factories
{
    internal class CompilerFactory
    {
        public static ICompiler Create()
        {
            return new CompileCCode(new CustomLogger<CompileCCode>());
        }
    }
    
    public class CustomLogger<T> : ILogger<T> where T : class
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }
    }
}
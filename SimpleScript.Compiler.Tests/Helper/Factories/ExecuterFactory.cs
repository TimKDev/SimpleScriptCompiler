using NSubstitute;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Adapter.C;

namespace SimpleScript.Compiler.Tests.Helper.Factories
{
    internal static class ExecuterFactory
    {
        public static IExecuter Create() => new Executer();
        public static IExecuter CreateMock() => Substitute.For<IExecuter>();
    }
}

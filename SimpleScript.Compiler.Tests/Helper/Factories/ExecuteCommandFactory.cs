namespace SimpleScript.Compiler.Tests.Helper.Factories
{
    internal class ExecuteCommandFactory
    {
        public static Command.ExecuteCommand Create()
        {
            return new Command.ExecuteCommand(CompilerServiceFactory.Create(), ExecuterFactory.CreateMock());
        }
    }
}
using ConsoleCore.Attributes;
using ConsoleCore.Interfaces;
using EntertainingErrors;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Compiler.Services;

namespace SimpleScript.Compiler.Command
{
    [Verb("execute", "Compiles and executes SimpleScript Code given a file path.")]
    public class ExecuteCommand : IConsoleCommand
    {
        private readonly ICompileService _compileService;
        private readonly IExecuter _executer;

        public ExecuteCommand(ICompileService compileService, IExecuter executer)
        {
            _compileService = compileService;
            _executer = executer;
        }

        public Result Execute(string[] args)
        {
            string? pathToCodeToCompile = args.FirstOrDefault();
            if (pathToCodeToCompile is null)
            {
                return Error.Create("Please provide the path to the .simple file as an input.");
            }

            var simpleScriptFileName = SimpleScriptFileName.Create(pathToCodeToCompile);
            if (!simpleScriptFileName.IsSuccess)
            {
                return Error.Create("Please provide the path to the .simple file as an input.");
            }

            var compilationResult =
                _compileService.CompileFromFile(pathToCodeToCompile, simpleScriptFileName.Value.ProgramName);
            if (!compilationResult.IsSuccess)
            {
                return compilationResult;
            }

            _executer.RunExecutable(simpleScriptFileName.Value.ProgramName);
            _compileService.Cleanup(simpleScriptFileName.Value.ProgramName);

            return Result.Success();
        }
    }
}
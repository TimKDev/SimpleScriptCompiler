using ConsoleCore.Attributes;
using ConsoleCore.Interfaces;
using EntertainingErrors;
using Microsoft.Extensions.Options;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Compiler.Services;

namespace SimpleScript.Compiler.Command
{
    [Verb("execute", "Compiles and executes SimpleScript Code given a file path.")]
    public class ExecuteCommand : IConsoleCommand
    {
        private readonly ICompileService _compileService;
        private readonly IExecuter _executer;
        private readonly CompilerSettings _compilerSettings;

        public ExecuteCommand(ICompileService compileService, IExecuter executer,
            IOptions<CompilerSettings> compilerSettings)
        {
            _compileService = compileService;
            _executer = executer;
            _compilerSettings = compilerSettings.Value;
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

            if (!_compilerSettings.CreateOutputFiles)
            {
                _compileService.Cleanup(simpleScriptFileName.Value.ProgramName);
            }

            return Result.Success();
        }
    }
}
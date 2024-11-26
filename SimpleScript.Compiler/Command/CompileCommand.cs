using ConsoleCore.Attributes;
using ConsoleCore.Interfaces;
using EntertainingErrors;
using SimpleScript.Compiler.Services;

namespace SimpleScript.Compiler.Command
{
    [Verb("compile", "Compiles SimpleScript Code.")]
    public class CompileCommand : IConsoleCommand
    {
        private readonly ICompileService _compileService;

        public CompileCommand(ICompileService compileService)
        {
            _compileService = compileService;
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

            var compilationResult = _compileService.Compile(pathToCodeToCompile, simpleScriptFileName.Value.ProgramName);
            if (!compilationResult.IsSuccess)
            {
                return compilationResult;
            }

            return Result.Success();
        }
    }
}
using ConsoleCore.Attributes;
using ConsoleCore.Interfaces;
using EntertainingErrors;
using Microsoft.Extensions.Options;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Compiler.Services;

namespace SimpleScript.Compiler.Command;

[Verb("execute-direct", "Compiles and executes SimpleScript Code.")]
public class ExecuteDirectCommand : IConsoleCommand
{
    private readonly ICompileService _compileService;
    private readonly IExecuter _executer;
    private readonly CompilerSettings _compilerSettings;

    public ExecuteDirectCommand(ICompileService compileService, IExecuter executer,
        IOptions<CompilerSettings> compilerSettings)
    {
        _compileService = compileService;
        _executer = executer;
        _compilerSettings = compilerSettings.Value;
    }

    public Result Execute(string[] args)
    {
        string? programToCompile = args.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(programToCompile))
        {
            return Error.Create("Please provide program code to compile.");
        }

        programToCompile = programToCompile.Replace('\'', '"');

        var programName = $"temp_file_{Guid.NewGuid()}";

        var compilationResult = _compileService.Compile(programName, programToCompile);
        if (!compilationResult.IsSuccess)
        {
            return compilationResult;
        }

        _executer.RunExecutable(programName);
        _compileService.Cleanup(programName);

        Console.WriteLine();
        Console.WriteLine();

        return Result.Success();
    }
}
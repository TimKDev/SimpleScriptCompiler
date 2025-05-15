using EntertainingErrors;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Lexer;
using SimpleScript.Lexer.Interfaces;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Compiler.Services;

public class CompileService : ICompileService
{
    private readonly ILexer _lexer;
    private readonly IParser _parser;
    private readonly IConverter _converter;
    private readonly ICompiler _compiler;


    public CompileService(ILexer lexer, IParser parser, IConverter converter, ICompiler compiler)
    {
        _lexer = lexer;
        _parser = parser;
        _converter = converter;
        _compiler = compiler;
    }

    public Result Compile(string programName, string code)
    {
        List<Token> programTokens = [];
        int currentLineNumber = 0;
        foreach (var line in code.Split(Environment.NewLine))
        {
            programTokens.AddRange(_lexer.ConvertToTokens(line, currentLineNumber));
            currentLineNumber++;
        }

        return CompileTokens(programName, programTokens);
    }

    public Result CompileFromFile(string pathToCodeToCompile, string programName)
    {
        string programAbsolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathToCodeToCompile);

        if (!File.Exists(programAbsolutePath))
        {
            return Error.Create("File not found.");
        }

        List<Token> programTokens = [];
        using StreamReader reader = new(programAbsolutePath);
        string? line;
        int currentLineNumber = 0;
        while ((line = reader.ReadLine()) != null)
        {
            programTokens.AddRange(_lexer.ConvertToTokens(line, currentLineNumber));
            currentLineNumber++;
        }

        return CompileTokens(programName, programTokens);
    }

    public void Cleanup(string programName)
    {
       _compiler.Cleanup(programName); 
    }

    private Result CompileTokens(string programName, List<Token> programTokens)
    {
        Result<ProgramNode> programNodeResult = _parser.ParseTokens(programTokens);
        if (!programNodeResult.IsSuccess)
        {
            return programNodeResult.Errors;
        }

        Result<string> convertedCode = _converter.ConvertToCCode(programNodeResult.Value);

        if (!convertedCode.IsSuccess)
        {
            return convertedCode.Errors;
        }

        if (!_compiler.Compile(programName, convertedCode.Value))
        {
            return Error.Create("Internal C Compilation failed");
        }

        return Result.Success();
    }
}
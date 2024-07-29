using ConsoleCore.Attributes;
using ConsoleCore.Interfaces;
using EntertainingErrors;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Lexer;
using SimpleScript.Lexer.Interfaces;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Compiler.Command
{
    [Verb("execute", "Compiles and executes SimpleScript Code.")]
    public class ExecuteCommand : IConsoleCommand
    {
        private readonly ILexer _lexer;
        private readonly IParser _parser;
        private readonly ICompiler _compiler;
        private readonly IConverter _converter;

        public ExecuteCommand(ICompiler compiler, ILexer lexer, IParser parser, IConverter converter)
        {
            _compiler = compiler;
            _lexer = lexer;
            _parser = parser;
            _converter = converter;
        }

        public Result Execute(string[] args)
        {
            string? pathToCodeToCompile = args.FirstOrDefault();
            if (pathToCodeToCompile is null || Path.GetExtension(pathToCodeToCompile) != ".simple")
            {
                return Error.Create("Please provide the path to the .simple file as an input.");
            }

            string programName = Path.GetFileNameWithoutExtension(pathToCodeToCompile);
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

            Result<ProgramNode> programNodeResult = _parser.ParseTokens(programTokens);
            if (!programNodeResult.IsSuccess)
            {
                return programNodeResult.Errors;
            }

            string convertedCode = _converter.ConvertToCCode(programNodeResult.Value);

            if (!_compiler.Compile(programName, convertedCode))
            {
                return Error.Create("Compilation failed");
            }

            Executer.RunExecutable(programName);

            return Result.Success();
        }
    }
}

using EntertainingErrors;

namespace SimpleScript.Compiler.Services;

public interface ICompileService
{
    Result Compile(string programName, string code);
    Result CompileFromFile(string pathToCodeToCompile, string programName);
    void Cleanup(string programName);
}
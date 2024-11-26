using EntertainingErrors;

namespace SimpleScript.Compiler.Services;

public interface ICompileService
{
    Result Compile(string pathToCodeToCompile, string programName);
}
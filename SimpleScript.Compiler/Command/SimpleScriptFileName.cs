using EntertainingErrors;

namespace SimpleScript.Compiler.Command;

public class SimpleScriptFileName
{
    private readonly string _path;
    public string ProgramName => Path.GetFileNameWithoutExtension(_path);

    private SimpleScriptFileName(string path)
    {
        _path = path;
    }

    public static Result<SimpleScriptFileName> Create(string path)
    {
        if (Path.GetExtension(path) != ".simple")
        {
            return Error.Create($"The path '{path}' is not a .simple file.");
        }

        return new SimpleScriptFileName(path);
    }
}
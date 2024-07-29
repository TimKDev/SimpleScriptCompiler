using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.Abstractions
{
    public interface IConverter
    {
        string ConvertToCCode(ProgramNode helloWorldProgramNode);
    }
}

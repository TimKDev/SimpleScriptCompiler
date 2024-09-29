using EntertainingErrors;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.Abstractions
{
    public interface IConverter
    {
        Result<string> ConvertToCCode(ProgramNode programNode);
    }
}
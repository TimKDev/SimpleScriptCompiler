using EntertainingErrors;

namespace SimpleScript.Parser.Nodes.Interfaces
{
    public interface IBaseNode
    {
        int StartLine { get; }
        int EndLine { get; }
        Error CreateError(string message);
    }
}
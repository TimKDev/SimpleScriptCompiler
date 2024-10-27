using EntertainingErrors;

namespace SimpleScript.Parser.Nodes.Interfaces
{
    public interface INodeBase
    {
        int StartLine { get; }
        int EndLine { get; }
        Error CreateError(string message);
    }
}
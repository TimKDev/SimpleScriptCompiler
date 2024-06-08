using EntertainingErrors;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;

namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Interfaces
{
    public interface INode
    {
        NodeTypes NodeType { get; }
        int StartLineNumber { get; }
        int EndLineNumber { get; }
        Result Validate();
    }
}

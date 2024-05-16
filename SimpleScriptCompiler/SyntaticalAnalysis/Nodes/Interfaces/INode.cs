using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;

namespace SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Interfaces
{
    public interface INode
    {
        public NodeTypes Type { get; }
    }
}

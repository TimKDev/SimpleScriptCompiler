using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Interfaces;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;

namespace SimpleScriptCompiler.SyntaticalAnalysis.Nodes
{
    public class ProgramNode
    {
        public NodeTypes Type => NodeTypes.Program;

        public List<INode> ChildNodes { get; } = [];
    }
}

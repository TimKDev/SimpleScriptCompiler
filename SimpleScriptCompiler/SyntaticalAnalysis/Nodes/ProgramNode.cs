using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Interfaces;

namespace SimpleScriptCompiler.SyntaticalAnalysis.Nodes
{
    public class ProgramNode
    {
        public NodeTypes Type => NodeTypes.Program;

        public List<INode> ChildNodes { get; } = new List<INode>();
    }
}

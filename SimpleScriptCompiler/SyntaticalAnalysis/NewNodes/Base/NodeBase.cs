using EntertainingErrors;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Interfaces;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;

namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Base
{
    public abstract class NodeBase : INode
    {
        public abstract NodeTypes NodeType { get; }
        protected abstract Result ValidateNode();
        public int StartLineNumber { get; }
        public int EndLineNumber { get; protected set; }
        protected NodeBase(int start, int end)
        {
            StartLineNumber = start;
            EndLineNumber = end;
        }

        public Result Validate()
        {
            return ValidateNode();
        }
    }
}

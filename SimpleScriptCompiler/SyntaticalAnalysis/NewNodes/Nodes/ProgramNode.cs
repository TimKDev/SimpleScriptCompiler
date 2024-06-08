using EntertainingErrors;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Base;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;

namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Nodes
{
    public class ProgramNode : HasChildNodeBase
    {
        public override NodeTypes NodeType => NodeTypes.Program;
        protected override List<NodeTypes> SupportedChildNodeTypes => [NodeTypes.VariableDeklaration];

        public ProgramNode(int start, int end) : base(start, end)
        {
        }

        protected override Result ValidateNode()
        {
            return Result.Success();
        }
    }
}

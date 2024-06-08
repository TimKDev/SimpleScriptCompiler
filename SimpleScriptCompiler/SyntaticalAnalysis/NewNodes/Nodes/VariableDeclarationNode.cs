using EntertainingErrors;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Base;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Interfaces;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;

namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Nodes
{
    public class VariableDeclarationNode : HasChildNodeBase, INode
    {
        public override NodeTypes NodeType => NodeTypes.VariableDeklaration;
        protected override List<NodeTypes> SupportedChildNodeTypes => [NodeTypes.Expression];
        public string VariableName { get; }

        public VariableDeclarationNode(string variableName, int start, int end) : base(start, end)
        {
            VariableName = variableName;
        }

        protected override Result ValidateNode()
        {
            return Result.Success();
        }
    }
}

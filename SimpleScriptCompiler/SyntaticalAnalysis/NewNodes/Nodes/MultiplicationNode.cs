using EntertainingErrors;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Base;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;

namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Nodes
{
    internal class MultiplicationNode : HasChildNodeBase
    {
        public override NodeTypes NodeType => NodeTypes.Multiplication;
        protected override List<NodeTypes> SupportedChildNodeTypes => [NodeTypes.Variable, NodeTypes.NumberValue, NodeTypes.Addition, NodeTypes.Multiplication];
        public MultiplicationNode(int start, int end) : base(start, end)
        {
        }

        protected override Result ValidateNode()
        {
            //Validiere Typen
            //Überprüfe das es sich um genau zwei ChildNodes handelt
            throw new NotImplementedException();
        }
    }
}

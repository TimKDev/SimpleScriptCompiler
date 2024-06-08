using EntertainingErrors;
using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Base;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;

namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Nodes
{
    public class ExpressionNode : HasChildNodeBase
    {
        public static TokenType[] SupportedTokenTypes = [
            TokenType.Number,
            TokenType.String,
            TokenType.Variable,
            TokenType.PLUS,
            TokenType.MINUS,
            TokenType.MULTIPLY,
            TokenType.DIVIDE,
            TokenType.POWER,
            TokenType.SMALLER,
            TokenType.GREATER,
            TokenType.SMALLER_OR_EQUAL,
            TokenType.GREATER_OR_EQUAL,
            TokenType.EQUAL,
            TokenType.OPEN_BRACKET,
            TokenType.CLOSED_BRACKET
        ];
        public override NodeTypes NodeType => NodeTypes.Expression;

        protected override List<NodeTypes> SupportedChildNodeTypes => throw new NotImplementedException();

        public ExpressionNode(int start, int end) : base(start, end)
        {
        }

        protected override Result ValidateNode()
        {
            throw new NotImplementedException();
        }
    }
}

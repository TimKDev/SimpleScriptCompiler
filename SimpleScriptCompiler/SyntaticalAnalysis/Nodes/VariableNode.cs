using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Interfaces;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Interfaces;

namespace SimpleScriptCompiler.SyntaticalAnalysis.Nodes
{
    public class VariableNode : INode, IExpressionPart
    {
        public NodeTypes Type => NodeTypes.Variable;
        public string Name { get; }

        private VariableNode(string name)
        {
            Name = name;
        }

        public static VariableNode CreateFromToken(Token token)
        {
            if (token.TokenType != TokenType.Variable)
            {
                throw new Exception();
            }

            if (token.Value == null)
            {
                throw new Exception();
            }

            return new(token.Value);
        }
    }
}

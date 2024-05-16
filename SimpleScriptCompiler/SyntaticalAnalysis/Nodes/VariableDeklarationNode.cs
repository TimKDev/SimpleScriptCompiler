using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Interfaces;

namespace SimpleScriptCompiler.SyntaticalAnalysis.Nodes
{
    public class VariableDeklarationNode : INode
    {
        public NodeTypes Type => NodeTypes.VariableDeklaration;
        public int StartLineNumber { get; }
        public int EndLineNumber { get; }
        public string Name { get; }
        public ExpressionNode? InitialValue { get; }

        private VariableDeklarationNode(string name, int start, int end, ExpressionNode? initialValue = null)
        {
            Name = name;
            InitialValue = initialValue;
            StartLineNumber = start;
            EndLineNumber = end;
        }

        public static VariableDeklarationNode CreateFromTokens(List<Token> tokens)
        {
            if (tokens.Count < 2 || tokens[0].TokenType != TokenType.LET || tokens[1].TokenType != TokenType.Variable)
            {
                throw new Exception();
            }

            string name = tokens[1].Value!;
            var start = tokens[0].Line;
            var end = tokens[^1].Line;

            if (tokens.Count > 2)
            {
                //Beispiel: LET test = a + b * 2
                var expressionNode = ExpressionNode.CreateByTokens(tokens.Skip(3).ToList());
                return new VariableDeklarationNode(name, start, end, expressionNode);
            }
            return new VariableDeklarationNode(name, start, end);
        }
    }
}

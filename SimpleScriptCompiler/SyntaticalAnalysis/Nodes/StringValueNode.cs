using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Interfaces;

namespace SimpleScriptCompiler.SyntaticalAnalysis.Nodes
{
    public class StringValueNode: INode, IExpressionPart
    {
        public NodeTypes Type => NodeTypes.StringValue;
        public string Value { get; }

        private StringValueNode(string value)
        {
            Value = value;
        }

        public static StringValueNode CreateFromToken(Token token)
        {
            if (token.TokenType != TokenType.String)
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

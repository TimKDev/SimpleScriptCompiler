using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Interfaces;

namespace SimpleScriptCompiler.SyntaticalAnalysis.Nodes
{
    public class NumberValueNode: INode, IExpressionPart
    {
        public NodeTypes Type => NodeTypes.NumberValue;
        public int Value { get; }

        private NumberValueNode(int value)
        {
            Value = value;
        }

        public static NumberValueNode CreateFromToken(Token token)
        {
            if(token.TokenType != TokenType.Number)
            {
                throw new Exception();
            }

            if(!int.TryParse(token.Value, out int value))
            {
                throw new Exception();
            }

            return new(value);
        }
    }
}

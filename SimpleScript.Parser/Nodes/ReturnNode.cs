using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class ReturnNode : NodeBase, IBodyNode
    {
        public IExpression NodeToReturn { get; private set; }

        private ReturnNode(IExpression nodeToReturn, int startLine,
            int endLine) : base(startLine, endLine)
        {
            NodeToReturn = nodeToReturn;
        }

        public static Result<ReturnNode> Create(IExpression expression)
        {
            return new ReturnNode(expression, expression.StartLine, expression.EndLine);
        }
    }
}
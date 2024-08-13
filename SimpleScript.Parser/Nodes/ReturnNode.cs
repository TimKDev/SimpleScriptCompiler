using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class ReturnNode : IBodyNode
    {
        public IExpression NodeToReturn { get; private set; }

        private ReturnNode(IExpression nodeToReturn)
        {
            NodeToReturn = nodeToReturn;
        }

        public static Result<ReturnNode> Create(IExpression expression)
        {
            return new ReturnNode(expression);
        }
    }
}

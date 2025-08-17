using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class FunctionInvocationNode : NodeBase, IBodyNode, IAddable, IMultiplyable, ISizeComparable
    {
        public string FunctionName { get; private set; }
        public IExpression[] FunctionArguments { get; private set; }

        public FunctionInvocationNode(string functionName, IExpression[] functionArguments, int startLine, int endLine)
            : base(startLine, endLine)
        {
            FunctionName = functionName;
            FunctionArguments = functionArguments;
        }
    }
}
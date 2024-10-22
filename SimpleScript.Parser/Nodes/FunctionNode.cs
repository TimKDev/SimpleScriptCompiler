using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class FunctionNode : NodeBase, IHasBodyNode, IBodyNode
    {
        public string Name { get; }
        public List<FunctionArgumentNode> Arguments { get; }
        public BodyNode Body { get; }

        public FunctionNode(string name, List<FunctionArgumentNode> arguments, BodyNode body, int startLine,
            int endLine) : base(startLine, endLine)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
        }

        public Result<ReturnType> GetReturnType(Scope functionScope)
        {
            var returnNodeExpression = Body.ChildNodes.FirstOrDefault(n => n is ReturnNode);
            if (returnNodeExpression is null || returnNodeExpression is not ReturnNode returnNode)
            {
                return ReturnType.Void;
            }

            var scopeValue = functionScope.GetScopeForExpression(returnNode.NodeToReturn);
            if (!scopeValue.IsSuccess)
            {
                return scopeValue.Errors;
            }

            return scopeValue.Value.ValueType switch
            {
                ValueTypes.Number => ReturnType.Int,
                ValueTypes.String => ReturnType.String,
                ValueTypes.Void => returnNode.CreateError("A Void value cannot be a return type."),
                _ => throw new NotImplementedException()
            };
        }
    }
}
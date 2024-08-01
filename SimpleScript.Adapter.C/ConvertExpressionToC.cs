using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C
{
    public static class ConvertExpressionToC
    {
        public static string Convert(IExpression expressionNode)
        {
            return expressionNode switch
            {
                StringNode stringNode => $"\"{stringNode.Value}\"",
                NumberNode numberNode => numberNode.Value.ToString(),
                VariableNode variableNode => variableNode.Name,
                AddNode addNode => $"({Convert(addNode.FirstArgument)} + {Convert(addNode.SecondArgument)})",
                MultiplyNode multiplyNode => $"({Convert(multiplyNode.FirstArgument)} * {Convert(multiplyNode.SecondArgument)})",
                _ => throw new NotImplementedException(),
            };
        }
    }
}



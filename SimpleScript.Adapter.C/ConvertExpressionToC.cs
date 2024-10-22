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
                MultiplyNode multiplyNode =>
                    $"({Convert(multiplyNode.FirstArgument)} * {Convert(multiplyNode.SecondArgument)})",
                FunctionInvocationNode functionInvocationNode => ConvertFunctionInvocationNode(functionInvocationNode),
                _ => throw new NotImplementedException(),
            };
        }

        private static string ConvertFunctionInvocationNode(FunctionInvocationNode functionInvocationNode)
        {
            var result = functionInvocationNode.FunctionName + "(";
            result += string.Join(", ", functionInvocationNode.FunctionArguments.Select(Convert));
            result += ")";

            return result;
        }
    }
}
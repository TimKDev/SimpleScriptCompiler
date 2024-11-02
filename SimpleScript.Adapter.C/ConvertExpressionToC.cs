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
                BooleanNode booleanNode => booleanNode.Value ? "true" : "false",
                VariableNode variableNode => variableNode.Name,
                AddNode addNode => $"({Convert(addNode.FirstArgument)} + {Convert(addNode.SecondArgument)})",
                MinusNode minusNode => $"({Convert(minusNode.FirstArgument)} - {Convert(minusNode.SecondArgument)})",
                MultiplyNode multiplyNode =>
                    $"({Convert(multiplyNode.FirstArgument)} * {Convert(multiplyNode.SecondArgument)})",
                EqualityNode equalityNode =>
                    $"({Convert(equalityNode.FirstArgument)} == {Convert(equalityNode.SecondArgument)})",
                InEqualityNode inEqualityNode =>
                    $"({Convert(inEqualityNode.FirstArgument)} != {Convert(inEqualityNode.SecondArgument)})",
                GreaterNode greaterNode =>
                    $"({Convert(greaterNode.FirstArgument)} > {Convert(greaterNode.SecondArgument)})",
                GreaterOrEqualNode greaterOrEqualNode =>
                    $"({Convert(greaterOrEqualNode.FirstArgument)} >= {Convert(greaterOrEqualNode.SecondArgument)})",
                SmallerNode smallerNode =>
                    $"({Convert(smallerNode.FirstArgument)} < {Convert(smallerNode.SecondArgument)})",
                SmallerOrEqualNode smallerOrEqualNode =>
                    $"({Convert(smallerOrEqualNode.FirstArgument)} < {Convert(smallerOrEqualNode.SecondArgument)})",
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
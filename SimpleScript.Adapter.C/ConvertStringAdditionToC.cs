using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C
{
    public static class ConvertStringAdditionToC
    {
        public static Result<string[]> Convert(string variableName, AddNode addNode, ScopeVariableEntry initialValueScope)
        {
            var result = new List<string>()
            {
                 $"char {variableName}[{initialValueScope.Lenght}];\n"
            };
            List<IAddable> variableOrStringNodes = GetChildNodesForStringAddition(addNode);
            if (initialValueScope.HeapAllocation)
            {
                return ConvertToStringAdditionWithHeapAllocation(variableName, variableOrStringNodes);
            }
            var appendedString = false;
            foreach (var node in variableOrStringNodes)
            {
                if (!appendedString)
                {
                    result.Add($"strcpy({variableName}, {TransformStringOrVariableNode(node)});\n");  
                    appendedString = true;
                    continue;
                }

                result.Add($"strcat({variableName}, {TransformStringOrVariableNode(node)});\n");
            }

            return result.ToArray();
        }

        private static Result<string[]> ConvertToStringAdditionWithHeapAllocation(string variableName, List<IAddable> nodesToAdd)
        {
            throw new NotImplementedException();
        }

        private static string TransformStringOrVariableNode(IAddable node)
        {
            return node switch
            {
                VariableNode variableNode => $"{variableNode.Name}",
                StringNode stringNode => $"\"{stringNode.Value}\"",
                _ => throw new NotImplementedException(),
            };
        }

        private static List<IAddable> GetChildNodesForStringAddition(IAddable node)
        {
            List<IAddable> variableOrStringNodes = [];

            switch (node)
            {
                case StringNode stringNode:
                    variableOrStringNodes.Add(stringNode);
                    break;
                case VariableNode variableNode:
                    variableOrStringNodes.Add(variableNode);
                    break;
                case AddNode addNode:
                    List<IAddable> firstArgResult = GetChildNodesForStringAddition(addNode.FirstArgument);
                    List<IAddable> secondArgResult = GetChildNodesForStringAddition(addNode.SecondArgument);
                    variableOrStringNodes.AddRange(firstArgResult);
                    variableOrStringNodes.AddRange(secondArgResult);
                    break;
                default:
                    throw new ArgumentException();
            }

            return variableOrStringNodes;
        }
    }
}



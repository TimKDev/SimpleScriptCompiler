using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C
{
    public static class ConvertStringAdditionToC
    {
        public static Result<string> Convert(string variableName, AddNode addNode, ScopeVariableEntry initialValueScope)
        {
            (List<StringNode> stringNodes, List<VariableNode> variableNodes) = GetChildNodesForStringAddition(addNode);
            string result = $"char {variableName}[{initialValueScope.Lenght}];\n";
            bool appendedString = false;
            foreach (StringNode stringNode in stringNodes)
            {
                if (!appendedString)
                {
                    result += $"strcpy({variableName}, \"{stringNode.Value}\");\n";
                    appendedString = true;
                    continue;
                }
                result += $"strcat({variableName}, \"{stringNode.Value}\");\n";
            }

            foreach (VariableNode variableNode in variableNodes)
            {
                if (!appendedString)
                {
                    result += $"strcpy({variableName}, {variableNode.Name});\n";
                    appendedString = true;
                    continue;
                }
                result += $"strcat({variableName}, {variableNode.Name});\n";
            }

            return result.TrimEnd();
        }

        private static (List<StringNode> StringNodes, List<VariableNode> VariableNodes) GetChildNodesForStringAddition(IAddable node)
        {
            List<StringNode> stringNodes = [];
            List<VariableNode> variableNodes = [];

            switch (node)
            {
                case StringNode stringNode:
                    stringNodes.Add(stringNode);
                    break;
                case VariableNode variableNode:
                    variableNodes.Add(variableNode);
                    break;
                case AddNode addNode:
                    (List<StringNode> firstStringNodes, List<VariableNode> firstVariableNodes) = GetChildNodesForStringAddition(addNode.FirstArgument);
                    (List<StringNode> secondStringNodes, List<VariableNode> secondVariableNodes) = GetChildNodesForStringAddition(addNode.SecondArgument);
                    stringNodes.AddRange(firstStringNodes);
                    variableNodes.AddRange(firstVariableNodes);
                    stringNodes.AddRange(secondStringNodes);
                    variableNodes.AddRange(secondVariableNodes);
                    break;
                default:
                    throw new ArgumentException();
            }

            return (stringNodes, variableNodes);
        }
    }
}



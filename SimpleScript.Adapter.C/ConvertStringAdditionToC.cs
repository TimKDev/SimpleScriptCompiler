﻿using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C
{
    public static class ConvertStringAdditionToC
    {
        public static Result<string> Convert(string variableName, AddNode addNode, ScopeVariableEntry initialValueScope)
        {
            List<IAddable> variableOrStringNodes = GetChildNodesForStringAddition(addNode);
            string result = $"char {variableName}[{initialValueScope.Lenght}];\n";
            bool appendedString = false;
            foreach (IAddable node in variableOrStringNodes)
            {
                if (!appendedString)
                {
                    result += $"strcpy({variableName}, {TransformStringOrVariableNode(node)});\n";
                    appendedString = true;
                    continue;
                }
                result += $"strcat({variableName}, {TransformStringOrVariableNode(node)});\n";
            }

            return result.TrimEnd();
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


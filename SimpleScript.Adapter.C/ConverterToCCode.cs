using EntertainingErrors;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C
{
    public class ConverterToCCode : IConverter
    {
        private static readonly string MainBodyTemplateVariable = "mainBody";
        private static readonly string MainCTemplate = @$"
                #include <stdio.h>
                #include <string.h>
                int main() {{
                    {{{MainBodyTemplateVariable}}}
                    return 0;
                }}
            ";

        public string ConvertToCCode(ProgramNode helloWorldProgramNode)
        {
            List<string> cMainScopeStatements = [];
            List<Error> errors = [];
            //TTODO Wo kommt der Scope hin? => Sollte ja eigentlich unabhängig vom Converter sein!
            Scope mainScope = new();
            foreach (IProgramRootNodes directProgramChild in helloWorldProgramNode.ChildNodes)
            {
                Result<string> createStatementResult = (directProgramChild switch
                {
                    PrintNode printNode => ConvertPrintNode(printNode, mainScope),
                    VariableDeclarationNode variableDeclarationNode => ConvertVariableDeklarationNode(variableDeclarationNode, mainScope),
                    _ => throw new NotImplementedException()
                });

                if (!createStatementResult.IsSuccess)
                {
                    errors.AddRange(createStatementResult.Errors);
                    continue;
                }

                cMainScopeStatements.Add(createStatementResult.Value);
            }

            return ReplaceTemplateVariable(MainCTemplate, MainBodyTemplateVariable, string.Join('\n', cMainScopeStatements));
        }

        private string ReplaceTemplateVariable(string template, string templateVariableName, string templateVariableValue)
        {
            return template.Replace("{" + templateVariableName + "}", templateVariableValue);
        }

        private Result<string> ConvertVariableDeklarationNode(VariableDeclarationNode variableDeclarationNode, Scope scope)
        {
            Result<ScopeVariableEntry> initialValueScope = scope.AddVariable(variableDeclarationNode);
            if (!initialValueScope.IsSuccess)
            {
                return initialValueScope.Errors;
            }

            return initialValueScope.Value.ValueType switch
            {
                ValueTypes.String => ConvertStringVariableDeclaration(variableDeclarationNode, initialValueScope.Value),
                ValueTypes.Number => $"int {variableDeclarationNode.VariableName} = {ConvertExpressionNode(variableDeclarationNode.InitialValue)};",
                _ => throw new NotImplementedException(),
            };
        }

        private Result<string> ConvertStringVariableDeclaration(VariableDeclarationNode variableDeclarationNode, ScopeVariableEntry initialValueScope)
        {
            if (variableDeclarationNode.InitialValue is AddNode addNode)
            {
                //Special Logic for C String Concat:
                return ConvertVariableDeklarationWithInitStringAddNode(variableDeclarationNode.VariableName, addNode, initialValueScope);
            }

            Result<string> convertExpressionResult = ConvertExpressionNode(variableDeclarationNode.InitialValue);
            if (!convertExpressionResult.IsSuccess)
            {
                return convertExpressionResult.Errors;
            }

            return $"char {variableDeclarationNode.VariableName}[] = {convertExpressionResult.Value};";
        }

        private Result<string> ConvertPrintNode(PrintNode printNode, Scope scope)
        {
            if (printNode.NodeToPrint is not IExpression nodeToPrint)
            {
                throw new NotImplementedException();
            }
            Result<ScopeVariableEntry> nodeToPrintScope = scope.GetScopeForExpression(nodeToPrint);
            if (!nodeToPrintScope.IsSuccess)
            {
                return nodeToPrintScope.Errors;
            }

            return nodeToPrintScope.Value.ValueType switch
            {
                ValueTypes.String when nodeToPrint is AddNode addNodeToPrint => ConvertPrintOfStringAddNode(addNodeToPrint, nodeToPrintScope.Value),
                ValueTypes.String => $"printf({ConvertExpressionNode(nodeToPrint)});",
                ValueTypes.Number => $"printf({ConvertExpressionNode(nodeToPrint)});",
                _ => throw new NotImplementedException(),
            };
        }

        private Result<string> ConvertExpressionNode(IExpression expressionNode)
        {
            return expressionNode switch
            {
                StringNode stringNode => $"\"{stringNode.Value}\"",
                NumberNode numberNode => numberNode.Value.ToString(),
                VariableNode variableNode => variableNode.Name,
                AddNode addNode => $"({ConvertExpressionNode(addNode.FirstArgument)} + {ConvertExpressionNode(addNode.SecondArgument)})",
                MultiplyNode multiplyNode => $"({ConvertExpressionNode(multiplyNode.FirstArgument)} * {ConvertExpressionNode(multiplyNode.SecondArgument)})",
                _ => throw new NotImplementedException(),
            };
        }

        private Result<string> ConvertPrintOfStringAddNode(AddNode addNode, ScopeVariableEntry nodeToPrintVariableScope)
        {
            //Verwende ConvertVariableDeklarationWithInitStringAddNode mit custom variablen namen!
            return "";
        }

        private Result<string> ConvertVariableDeklarationWithInitStringAddNode(string variableName, AddNode addNode, ScopeVariableEntry initialValueScope)
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

            return result;
        }

        private (List<StringNode> StringNodes, List<VariableNode> VariableNodes) GetChildNodesForStringAddition(IAddable node)
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

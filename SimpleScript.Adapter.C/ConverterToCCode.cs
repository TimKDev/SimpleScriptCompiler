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
                int main() {{
                    {{{MainBodyTemplateVariable}}}
                    return 0;
                }}
            ";

        public string ConvertToCCode(ProgramNode helloWorldProgramNode)
        {
            List<string> cMainScopeStatements = [];
            List<Error> errors = [];
            Scope mainScope = new();
            foreach (IProgramRootNodes directProgramChild in helloWorldProgramNode.ChildNodes)
            {
                Result<string> createStatementResult = (directProgramChild switch
                {
                    PrintNode printNode => ConvertPrintNode(printNode),
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
            Result<ValueTypes> getVariableTypeResult = scope.AddVariable(variableDeclarationNode);
            if (!getVariableTypeResult.IsSuccess)
            {
                return getVariableTypeResult.Errors;
            }

            return getVariableTypeResult.Value switch
            {
                ValueTypes.String => $"char {variableDeclarationNode.VariableName}[] = {ConvertPrintableNode(variableDeclarationNode.InitialValue)};",
                ValueTypes.Number => $"int {variableDeclarationNode.VariableName} = {ConvertPrintableNode(variableDeclarationNode.InitialValue)};",
                _ => throw new NotImplementedException(),
            };
        }

        private Result<string> ConvertPrintNode(PrintNode printNode)
        {
            return $"printf({ConvertPrintableNode(printNode.NodeToPrint)});";
        }

        private string ConvertPrintableNode(IPrintableNode printableNode)
        {
            return printableNode switch
            {
                StringNode stringNode => $"\"{stringNode.Value}\"",
                NumberNode numberNode => numberNode.Value.ToString(),
                VariableNode variableNode => variableNode.Name,
                AddNode addNode => $"({ConvertPrintableNode(addNode.FirstArgument)} + {ConvertPrintableNode(addNode.SecondArgument)})",
                MultiplyNode multiplyNode => $"({ConvertPrintableNode(multiplyNode.FirstArgument)} * {ConvertPrintableNode(multiplyNode.SecondArgument)})",
                _ => throw new NotImplementedException(),
            };
        }
    }
}

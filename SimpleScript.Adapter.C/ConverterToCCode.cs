using SimpleScript.Adapter.Abstractions;
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
            List<string> mainScopeStatements = [];
            foreach (IProgramRootNodes directProgramChild in helloWorldProgramNode.ChildNodes)
            {
                mainScopeStatements.Add(directProgramChild switch
                {
                    PrintNode printNode => $"printf({ConvertPrintableNode(printNode.NodeToPrint)});",
                    VariableDeclarationNode variableDeclarationNode => ConvertVariableDeklarationNode(variableDeclarationNode),
                    _ => throw new NotImplementedException()
                });
            }

            return ReplaceTemplateVariable(MainCTemplate, MainBodyTemplateVariable, string.Join('\n', mainScopeStatements));
        }

        private string ReplaceTemplateVariable(string template, string templateVariableName, string templateVariableValue)
        {
            return template.Replace("{" + templateVariableName + "}", templateVariableValue);
        }

        private string ConvertVariableDeklarationNode(VariableDeclarationNode variableDeclarationNode)
        {
            return variableDeclarationNode.InitialValue switch
            {
                StringNode stringNode => $"char {variableDeclarationNode.VariableName}[] = {ConvertPrintableNode(stringNode)};",
                NumberNode numberNode => $"int {variableDeclarationNode.VariableName} = {numberNode.Value};",
                _ => throw new NotImplementedException(),
            };
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

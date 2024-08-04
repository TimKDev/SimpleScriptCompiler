using EntertainingErrors;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C
{
    public class ProgramConverterToC : IConverter
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

        public Result<string> ConvertToCCode(ProgramNode helloWorldProgramNode)
        {
            List<string> cMainScopeStatements = [];
            List<Error> errors = [];
            Scope mainScope = new();
            foreach (IProgramRootNodes directProgramChild in helloWorldProgramNode.ChildNodes)
            {
                Result<string> createStatementResult = directProgramChild switch
                {
                    PrintNode printNode => ConvertPrintNodeToC.Convert(printNode, mainScope),
                    VariableDeclarationNode variableDeclarationNode => ConvertVariableDeklarationToC.Convert(variableDeclarationNode, mainScope),
                    InputNode inputNode => ConvertInputNodeToC.Convert(inputNode, mainScope),
                    _ => throw new NotImplementedException()
                };

                if (!createStatementResult.IsSuccess)
                {
                    errors.AddRange(createStatementResult.Errors);
                    continue;
                }

                cMainScopeStatements.Add(createStatementResult.Value);
            }

            if (errors.Any())
            {
                return errors;
            }

            return ReplaceTemplateVariable(MainCTemplate, MainBodyTemplateVariable, string.Join('\n', cMainScopeStatements));
        }

        private string ReplaceTemplateVariable(string template, string templateVariableName, string templateVariableValue)
        {
            return template.Replace("{" + templateVariableName + "}", templateVariableValue);
        }


    }
}



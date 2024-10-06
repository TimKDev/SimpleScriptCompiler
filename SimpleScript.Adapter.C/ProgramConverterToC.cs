using EntertainingErrors;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C
{
    public class ProgramConverterToC : IConverter
    {
        private static readonly string MainBodyTemplateVariable = "mainBody";
        private static readonly string FunctionDeclarationTemplateVariable = "functionDeclaration";
        private static readonly string MainCTemplate = @$"
                #include <stdio.h>
                #include <string.h>
                #include <stdlib.h>
                #include <compiler-helper.h>
                {{{FunctionDeclarationTemplateVariable}}}
                int main() {{
                    {{{MainBodyTemplateVariable}}}
                    return 0;
                }}
            ";

        public Result<string> ConvertToCCode(ProgramNode programNode)
        {
            Result<(List<string> mainStatements, List<string> cFunctionDeclarations)> convertionResult = ConvertBodyNodeToC.ConvertToStatements(programNode.Body);

            if (!convertionResult.IsSuccess)
            {
                return convertionResult.Errors;
            }

            (var cMainScopeStatements, List<string> cFunctionDeclaration) = convertionResult.Value;

            var stringWithReplacedBody = ReplaceTemplateVariable(MainCTemplate, MainBodyTemplateVariable, string.Join('\n', cMainScopeStatements));

            return ReplaceTemplateVariable(stringWithReplacedBody, FunctionDeclarationTemplateVariable, string.Join('\n', cFunctionDeclaration));
        }


        private static string ReplaceTemplateVariable(string template, string templateVariableName, string templateVariableValue)
        {
            return template.Replace("{" + templateVariableName + "}", templateVariableValue);
        }
    }
}



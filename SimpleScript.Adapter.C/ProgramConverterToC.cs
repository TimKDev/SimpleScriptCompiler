using EntertainingErrors;
using SimpleScript.Adapter.Abstractions;
using SimpleScript.Parser.Nodes;

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

        public Result<string> ConvertToCCode(ProgramNode programNode)
        {
            var convertionResult = ConvertBodyNodeToC.ConvertToStatements(programNode.Body);

            if (!convertionResult.IsSuccess)
            {
                return convertionResult.Errors;
            }

            (List<string> cMainScopeStatements, List<string> cFunctionDeclaration) = convertionResult.Value;

            return ReplaceTemplateVariable(MainCTemplate, MainBodyTemplateVariable, string.Join('\n', cMainScopeStatements));
        }


        private static string ReplaceTemplateVariable(string template, string templateVariableName, string templateVariableValue)
        {
            return template.Replace("{" + templateVariableName + "}", templateVariableValue);
        }
    }
}



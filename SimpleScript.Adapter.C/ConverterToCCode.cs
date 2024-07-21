using SimpleScript.Parser.Nodes;

namespace SimpleScript.Converter.C
{
    public class ConverterToCCode
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
            string printStatement = "printf(\"Hello World!\\n\");";
            return ReplaceTemplateVariable(MainCTemplate, MainBodyTemplateVariable, printStatement);
        }

        private string ReplaceTemplateVariable(string template, string templateVariableName, string templateVariableValue)
        {
            return template.Replace("{" + templateVariableName + "}", templateVariableValue);
        }
    }
}

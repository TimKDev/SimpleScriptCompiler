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
                typedef struct Node {{
                    void *data;
                    struct Node *next;
                }} Node;
                Node *head = NULL;
                void add_to_list(void *data) {{
                    Node *new_node = (Node *)malloc(sizeof(Node));
                    new_node->data = data;
                    new_node->next = head;
                    head = new_node;
                }}
                void free_list() {{
                    Node *current = head;
                    Node *next_node;
                    while (current != NULL) {{
                        next_node = current->next;
                        free(current->data);
                        free(current);
                        current = next_node;
                    }}
                    head = NULL;
                }}
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

            (List<string> cMainScopeStatements, List<string> cFunctionDeclaration) = convertionResult.Value;

            string stringWithReplacedBody = ReplaceTemplateVariable(MainCTemplate, MainBodyTemplateVariable, string.Join('\n', cMainScopeStatements));

            return ReplaceTemplateVariable(stringWithReplacedBody, FunctionDeclarationTemplateVariable, string.Join('\n', cFunctionDeclaration));
        }


        private static string ReplaceTemplateVariable(string template, string templateVariableName, string templateVariableValue)
        {
            return template.Replace("{" + templateVariableName + "}", templateVariableValue);
        }
    }
}



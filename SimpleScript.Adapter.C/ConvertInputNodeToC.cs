using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C
{
    internal static class ConvertInputNodeToC
    {
        public static Result<string> Convert(InputNode inputNode, Scope scope)
        {
            bool doesVariableExists = scope.DoesVariableNameExists(inputNode.VariableName);
            Result<ScopeVariableEntry> initialValueScope = scope.AddVariableScopeEntry(inputNode);
            if (!initialValueScope.IsSuccess)
            {
                return initialValueScope.Errors;
            }

            string tempVariableName = scope.GetTempVariableName();

            return CreateCInput(tempVariableName, scope) + (doesVariableExists ? $"\n{inputNode.VariableName} = {tempVariableName};" : $"\n char *{inputNode.VariableName} = {tempVariableName};");
        }

        private static string CreateCInput(string variableName, Scope scope)
        {
            string tempVariableLengthName = scope.GetTempVariableName();
            return @$" char {variableName}[{InputNode.CharLength}];
                fgets({variableName}, sizeof({variableName}), stdin);
                size_t {tempVariableLengthName} = strlen({variableName});
                if ({tempVariableLengthName} > 0 && {variableName}[{tempVariableLengthName} - 1] == '\n') 
                {{
                    {variableName}[{tempVariableLengthName} - 1] = '\0';
                }}";
        }
    }
}

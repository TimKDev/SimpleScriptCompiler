using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C
{
    public static class ConvertVariableDeklarationToC
    {
        public static Result<string[]> Convert(VariableDeclarationNode variableDeclarationNode, Scope scope)
        {
            bool doesVariableExists = scope.DoesVariableNameExists(variableDeclarationNode.VariableName);
            Result<ScopeVariableEntry> initialValueScope = scope.AddOrUpdateVariableScopeEntry(variableDeclarationNode);
            if (!initialValueScope.IsSuccess)
            {
                return initialValueScope.Errors;
            }

            var variableDeclarationExpression = initialValueScope.Value.ValueType switch
            {
                ValueTypes.String => ConvertStringVariableDeclaration(variableDeclarationNode, initialValueScope.Value, scope, doesVariableExists),
                ValueTypes.Number => ConvertNumberVariableDeclaration(variableDeclarationNode, doesVariableExists),
                _ => throw new NotImplementedException(),
            };

            return variableDeclarationExpression.Convert(item => new string[] { item });
        }

        private static Result<string> ConvertNumberVariableDeclaration(VariableDeclarationNode variableDeclarationNode, bool doesVariableExists)
        {
            string assertResult = $"{variableDeclarationNode.VariableName} = {ConvertExpressionToC.Convert(variableDeclarationNode.InitialValue)};";
            return doesVariableExists ? assertResult : $"int {assertResult}";
        }

        private static Result<string> ConvertStringVariableDeclaration(VariableDeclarationNode variableDeclarationNode, ScopeVariableEntry initialValueScope, Scope scope, bool doesVariableExists)
        {
            if (variableDeclarationNode.InitialValue is AddNode addNode)
            {
                string tempVariableName = scope.GetTempVariableName();
                return ConvertStringAdditionToC.Convert(tempVariableName, addNode, initialValueScope).MapIfSuccess<string>(result =>
                {
                    if (!doesVariableExists)
                    {
                        return $"{result}\nchar *{variableDeclarationNode.VariableName} = {tempVariableName};";
                    }
                    return $"{result}\n{variableDeclarationNode.VariableName} = {tempVariableName};";
                });
            }

            string assertResult = $"{variableDeclarationNode.VariableName} = {ConvertExpressionToC.Convert(variableDeclarationNode.InitialValue)};";

            return doesVariableExists ? assertResult : $"char *{assertResult}";
        }
    }
}



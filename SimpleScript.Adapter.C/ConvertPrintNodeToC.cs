using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C
{
    public static class ConvertPrintNodeToC
    {
        public static Result<string[]> Convert(PrintNode printNode, Scope scope)
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

            if (nodeToPrintScope.Value.ValueType is ValueTypes.String && nodeToPrint is AddNode addNodeToPrint)
            {
                return ConvertPrintOfStringAddNode(addNodeToPrint, nodeToPrintScope.Value, scope);
            }

            var printNodeExpression = nodeToPrintScope.Value.ValueType switch
            {
                ValueTypes.String => ConvertPrintOfString(nodeToPrint),
                ValueTypes.Number => ConvertPrintOfNumber(nodeToPrint),
                _ => throw new NotImplementedException(),
            };

            return new string[] { printNodeExpression };
        }

        private static string ConvertPrintOfString(IExpression nodeToPrint)
        {
            return $"printf({ConvertExpressionToC.Convert(nodeToPrint)});";
        }

        private static string ConvertPrintOfNumber(IExpression nodeToPrint)
        {
            return $"printf({ConvertExpressionToC.Convert(nodeToPrint)});";
        }

        private static Result<string[]> ConvertPrintOfStringAddNode(AddNode addNode,
            ScopeVariableEntry nodeToPrintVariableScope, Scope scope)
        {
            string tempVariableName = scope.GetTempVariableName();
            var stringAdditionResult =
                ConvertStringAdditionToC.Convert(tempVariableName, addNode, nodeToPrintVariableScope);
            if (!stringAdditionResult.IsSuccess)
            {
                return stringAdditionResult;
            }

            var result = stringAdditionResult.Value.ToList();
            result.Add($"\nprintf({tempVariableName});");
            return result.ToArray();
        }
    }
}
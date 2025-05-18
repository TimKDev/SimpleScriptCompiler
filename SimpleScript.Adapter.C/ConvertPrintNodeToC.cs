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

            var printNodeExpressionResult = PrintNodeExpression(nodeToPrintScope.Value, nodeToPrint, scope);

            if (!printNodeExpressionResult.IsSuccess)
            {
                return printNodeExpressionResult;
            }

            var result = new List<string>();
            result.AddRange(printNodeExpressionResult.Value);
            result.Add("fflush(stdout);");
            return result.ToArray();
        }

        private static Result<string[]> PrintNodeExpression(ScopeVariableEntry nodeToPrintScope,
            IExpression nodeToPrint,
            Scope scope)
        {
            if (nodeToPrintScope.ValueType is ValueTypes.String && nodeToPrint is AddNode addNodeToPrint)
            {
                return ConvertPrintOfStringAddNode(addNodeToPrint, nodeToPrintScope, scope);
            }

            var standardPrintNodeResult = nodeToPrintScope.ValueType switch
            {
                ValueTypes.String => ConvertPrintOfString(nodeToPrint),
                ValueTypes.Number => ConvertPrintOfNumber(nodeToPrint),
                ValueTypes.Boolean => ConvertPrintOfBoolean(nodeToPrint),
                _ => throw new NotImplementedException(),
            };

            return new[] { standardPrintNodeResult };
        }

        private static string ConvertPrintOfBoolean(IExpression nodeToPrint)
        {
            return $"printf({ConvertExpressionToC.Convert(nodeToPrint)} ? \"TRUE\" : \"FALSE\");";
        }

        private static string ConvertPrintOfString(IExpression nodeToPrint)
        {
            return $"printf(\"%s\", {ConvertExpressionToC.Convert(nodeToPrint)});";
        }

        private static string ConvertPrintOfNumber(IExpression nodeToPrint)
        {
            return $"printf(\"%d\", {ConvertExpressionToC.Convert(nodeToPrint)});";
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
            result.Add($"printf(\"%s\", {tempVariableName});");
            return result.ToArray();
        }
    }
}
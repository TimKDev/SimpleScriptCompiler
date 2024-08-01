using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C
{
    public static class ConvertPrintNodeToC
    {
        public static Result<string> Convert(PrintNode printNode, Scope scope)
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

            return nodeToPrintScope.Value.ValueType switch
            {
                ValueTypes.String when nodeToPrint is AddNode addNodeToPrint => ConvertPrintOfStringAddNode(addNodeToPrint, nodeToPrintScope.Value, scope),
                ValueTypes.String => ConvertPrintOfString(nodeToPrint),
                ValueTypes.Number => ConvertPrintOfNumber(nodeToPrint),
                _ => throw new NotImplementedException(),
            };
        }

        private static string ConvertPrintOfString(IExpression nodeToPrint)
        {
            return $"printf({ConvertExpressionToC.Convert(nodeToPrint)});";
        }

        private static string ConvertPrintOfNumber(IExpression nodeToPrint)
        {
            return $"printf({ConvertExpressionToC.Convert(nodeToPrint)});";
        }

        private static Result<string> ConvertPrintOfStringAddNode(AddNode addNode, ScopeVariableEntry nodeToPrintVariableScope, Scope scope)
        {
            string tempVariableName = scope.GetTempVariableName();
            Result<string> stringAdditionResult = ConvertStringAdditionToC.Convert(tempVariableName, addNode, nodeToPrintVariableScope);
            if (!stringAdditionResult.IsSuccess)
            {
                return stringAdditionResult;
            }

            return stringAdditionResult.Value + $"\nprintf({tempVariableName});"; ;
        }
    }
}



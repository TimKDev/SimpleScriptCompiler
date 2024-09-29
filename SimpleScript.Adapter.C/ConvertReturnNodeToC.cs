using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C
{
    public class ConvertReturnNodeToC
    {
        public static Result<string[]> Convert(ReturnNode returnNode, Scope mainScope)
        {
            var nodeToReturnScopeEntry = mainScope.GetScopeForExpression(returnNode.NodeToReturn);
            if (!nodeToReturnScopeEntry.IsSuccess)
            {
                return nodeToReturnScopeEntry.Errors;
            }
            //Zusätzlich müssen alle Variablendeklarationen mit String und Length 0 (Heapallocations) die nicht returned werden, mit free freigegeben werden, bevor die Funktion returned.

            //Falls eine String Variable mit Length 0 returned wird, muss diese in die Linked List gegeben werden mit add_to_list().

            //Hierfür sollte eine eigene Klasse mit potentiellen Helperfunctions geschrieben werden. 
            Result<string[]> returnNodeString;
            if (nodeToReturnScopeEntry.Value.ValueType is ValueTypes.String &&
                returnNode.NodeToReturn is AddNode addNode)
            {
                returnNodeString = ConvertStringAdditionReturnNode(addNode, nodeToReturnScopeEntry.Value, mainScope);
            }
            else
            {
                returnNodeString = new[] { $"return {ConvertExpressionToC.Convert(returnNode.NodeToReturn)};" };
            }

            return returnNodeString.IsSuccess ? returnNodeString.Value : returnNodeString.Errors;
        }

        private static Result<string[]> ConvertStringAdditionReturnNode(AddNode addNode,
            ScopeVariableEntry valueToReturnVariableScope, Scope scope)
        {
            var tempVariableName = scope.GetTempVariableName();
            var stringAdditionResult =
                ConvertStringAdditionToC.Convert(tempVariableName, addNode, valueToReturnVariableScope);
            if (!stringAdditionResult.IsSuccess)
            {
                return stringAdditionResult.Errors;
            }

            var result = stringAdditionResult.Value.ToList();
            result.Add($"\nreturn {tempVariableName};");
            return result.ToArray();
        }
    }
}
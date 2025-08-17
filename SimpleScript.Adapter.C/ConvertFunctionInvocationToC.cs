using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C;

internal static class ConvertFunctionInvocationToC
{
    internal static Result<string[]> Convert(FunctionInvocationNode functionInvocationNode, Scope scope)
    {
        var functionScopeEntry = scope.GetScopeEntry(functionInvocationNode.FunctionName);
        if (!functionScopeEntry.IsSuccess)
        {
            return functionInvocationNode.CreateError("Function name is unknown.");
        }

        string[] result = 
        [
            $"{functionInvocationNode.FunctionName}();",
        ];

        return result;
    }
}
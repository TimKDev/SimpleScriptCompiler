using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C;

internal class ConvertIfConditionToC
{
    internal static Result<string[]> Convert(IfNode ifNode, Scope scope)
    {
        var condition = ConvertExpressionToC.Convert(ifNode.Condition);
        var bodyResult = ConvertBodyNodeToC.ConvertToStatements(ifNode.Body, scope);

        if (!bodyResult.IsSuccess)
        {
            return bodyResult.Errors;
        }

        if (bodyResult.Value.cFunctionDeclarations.Count > 0)
        {
            return Error.Create("Function declarations inside if conditions are not supported.");
        }

        List<string> result = [$"if({condition})", "{"];
        result.AddRange(bodyResult.Value.mainStatements);
        result.Add("}");

        return result.ToArray();
    }
}
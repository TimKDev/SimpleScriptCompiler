using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C;

internal class ConvertWhileLoopToC
{
    internal static Result<string[]> Convert(WhileNode whileNode, Scope scope)
    {
        var scopeOfCondition = scope.GetScopeForExpression(whileNode.Condition);
        if (!scopeOfCondition.IsSuccess)
        {
            return scopeOfCondition.Errors;
        }

        var condition = ConvertExpressionToC.Convert(whileNode.Condition);
        var bodyResult = ConvertBodyNodeToC.ConvertToStatements(whileNode.Body, scope);

        if (!bodyResult.IsSuccess)
        {
            return bodyResult.Errors;
        }

        if (bodyResult.Value.cFunctionDeclarations.Count > 0)
        {
            return Error.Create("Function declarations inside while conditions are not supported.");
        }

        List<string> result = [$"while({condition})", "{"];
        result.AddRange(bodyResult.Value.mainStatements);
        result.Add("}");

        return result.ToArray();
    }
}
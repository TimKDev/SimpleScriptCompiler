using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C
{
    internal static class ConvertFunctionNodeToC
    {
        internal static Result<string[]> Convert(FunctionNode functionNode, Scope mainScope)
        {
            if (mainScope.DoesVariableNameExists(functionNode.Name))
            {
                return functionNode.CreateError(
                    $"The name {functionNode.Name} already exists and cannot be used again.");
            }


            Scope functionScope = new();
            foreach (FunctionArgumentNode arg in functionNode.Arguments)
            {
                functionScope.AddVariableScopeEntry(arg);
            }

            Result<(List<string> mainStatements, List<string> cFunctionDeclarations)> convertedFunctionBodyResult =
                ConvertBodyNodeToC.ConvertToStatements(functionNode.Body, functionScope);
            if (!convertedFunctionBodyResult.IsSuccess)
            {
                return convertedFunctionBodyResult.Errors;
            }

            (List<string> mainStatements, List<string> cFunctionDeclarations) = convertedFunctionBodyResult.Value;
            if (cFunctionDeclarations.Count != 0)
            {
                return functionNode.Body.CreateError(
                    "Functions can only be declared in the main scope of the program. Declaring function inside of other functions is not supported.");
            }

            var returnTypeResult = functionNode.GetReturnType(functionScope);
            if (!returnTypeResult.IsSuccess)
            {
                return returnTypeResult.Errors;
            }

            mainScope.AddVariableScopeEntry(functionNode, returnTypeResult.Value);

            var functionArgs = $"({string.Join(", ", functionNode.Arguments.Select(ConvertArgumentsToC))})";
            var returnType = ConvertReturnTypeToC(returnTypeResult.Value);
            var functionHeader = $"{returnType} {functionNode.Name}{functionArgs}";

            var result = new List<string>();
            result.Add(functionHeader);
            result.Add("{");
            result.AddRange(mainStatements);
            result.Add("}");

            return result.ToArray();
        }

        private static string ConvertReturnTypeToC(ReturnType returnType)
        {
            return returnType switch
            {
                ReturnType.Void => "void",
                ReturnType.Int => "int",
                ReturnType.String => "char *",
                _ => throw new ArgumentOutOfRangeException(nameof(returnType), returnType, null)
            };
        }

        private static string ConvertArgumentsToC(FunctionArgumentNode argumentNode)
        {
            return argumentNode.ArgumentType switch
            {
                ArgumentType.Int => $"int {argumentNode.ArgumentName}",
                ArgumentType.String => $"char {argumentNode.ArgumentName}[]",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
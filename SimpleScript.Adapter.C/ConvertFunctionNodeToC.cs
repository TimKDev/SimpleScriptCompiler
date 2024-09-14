using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C
{
    internal class ConvertFunctionNodeToC
    {
        internal static Result<string[]> Convert(FunctionNode functionNode, Scope mainScope)
        {
            if (mainScope.DoesVariableNameExists(functionNode.Name))
            {
                return functionNode.CreateError($"The name {functionNode.Name} already exists and cannot be used again.");
            }

            mainScope.AddVariableScopeEntry(functionNode);

            Scope functionScope = new();
            foreach (FunctionArgumentNode arg in functionNode.Arguments)
            {
                functionScope.AddVariableScopeEntry(arg);
            }

            Result<(List<string> mainStatements, List<string> cFunctionDeclarations)> convertedFunctionBodyResult = ConvertBodyNodeToC.ConvertToStatements(functionNode.Body, functionScope);
            if (!convertedFunctionBodyResult.IsSuccess)
            {
                return convertedFunctionBodyResult.Errors;
            }

            (List<string> mainStatements, List<string> cFunctionDeclarations) = convertedFunctionBodyResult.Value;
            if (cFunctionDeclarations.Count != 0)
            {
                return functionNode.Body.CreateError("Functions can only be declared in the main scope of the program. Declaring function inside of other functions is not supported.");
            }

            //Danach sollten alle Variablen in der Funktion im Scope hinterlegt sein, sodass falls die Funktion einen Returntyp besitzt, dieser aus der Expression des Returntypes bestimmt werden kann. => Damit bekommtt man den Typen für die Funktionssignatur.
            throw new NotImplementedException();
        }
    }
}

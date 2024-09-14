using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C
{
    internal class ConvertFunctionNodeToC
    {
        internal static Result<string[]> Convert(FunctionNode functionNode, Scope mainScope)
        {
            //Verifiziere, dass der Funktionsname noch nicht im Scope existiert, sonst Error 
            if (mainScope.DoesVariableNameExists(functionNode.Name))
            {
                return functionNode.CreateError($"The name {functionNode.Name} already exists and cannot be used again.");
            }

            //Füge den Funktionsnamen zum Scope hinzu
            mainScope.AddVariableScopeEntry(functionNode);

            //Parse den Funktionsbody analog wie beim Program Start, aber mit einem neuen Scope, der initial nur die Argumente der 
            //Funktion enthält. Achtung abstrahiere diesen Code, sodass er nur einmal geschrieben wird, da er sich häufig ändern wird und
            //beachte, dass Funktionsdeklarationen innerhalb einer anderen Funktion in C nicht supported wird. Daher werden ich dies in 
            //der ersten Version von SimpleScript auch nicht ermöglichen.
            var functionScope = new Scope();

            //Danach sollten alle Variablen in der Funktion im Scope hinterlegt sein, sodass falls die Funktion einen Returntyp besitzt, dieser aus der Expression des Returntypes bestimmt werden kann. => Damit bekommtt man den Typen für die Funktionssignatur.
        }
    }
}

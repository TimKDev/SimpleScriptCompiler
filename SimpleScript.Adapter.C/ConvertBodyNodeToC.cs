using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C
{
    public class ConvertBodyNodeToC
    {
        public static Result<(List<string> mainStatements, List<string> cFunctionDeclarations)> ConvertToStatements(
            BodyNode nodeWithBody, Scope? bodyScope = null)
        {
            List<string> cMainScopeStatements = [];
            List<string> cFunctionDeclarations = [];
            List<Error> errors = [];
            Scope mainScope = bodyScope ?? new();

            foreach (IBodyNode directProgramChild in nodeWithBody.ChildNodes)
            {
                //Sammel alle Function Node die Child Elemente der Body Node sind ein und deklariere die entsprechende Funktion.
                //Hierbei muss wieder auf den Scope geachtet werden, da zwei Funktionen mit dem gleichen Namen nicht erlaubt sind.
                Result<string[]> createStatementResult = directProgramChild switch
                {
                    PrintNode printNode => ConvertPrintNodeToC.Convert(printNode, mainScope),
                    VariableDeclarationNode variableDeclarationNode => ConvertVariableDeklarationToC.Convert(
                        variableDeclarationNode, mainScope),
                    InputNode inputNode => ConvertInputNodeToC.Convert(inputNode, mainScope),
                    FunctionNode functionNode => ConvertFunctionNodeToC.Convert(functionNode, mainScope),
                    ReturnNode returnNode => ConvertReturnNodeToC.Convert(returnNode, mainScope),
                    IfNode ifNode => ConvertIfConditionToC.Convert(ifNode, mainScope),
                    WhileNode whileNode => ConvertWhileLoopToC.Convert(whileNode, mainScope),
                  _ => throw new NotImplementedException()
                };

                if (!createStatementResult.IsSuccess)
                {
                    errors.AddRange(createStatementResult.Errors);
                    continue;
                }

                if (directProgramChild is FunctionNode)
                {
                    cFunctionDeclarations.AddRange(createStatementResult.Value);
                }
                else
                {
                    cMainScopeStatements.AddRange(createStatementResult.Value);
                }
            }

            return errors.Any() ? errors : (cMainScopeStatements, cFunctionDeclarations);
        }
    }
}
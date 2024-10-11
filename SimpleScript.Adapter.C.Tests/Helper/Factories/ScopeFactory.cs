using SimpleScript.Adapter.C.Tests.Helper.Factories;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C.Tests.ConverterTests
{
    internal class ScopeFactory
    {
        public static Scope Create((string name, IExpression expression)[] variableDeclarations,
            IList<FunctionArgumentNode>? functionArguments = null)
        {
            var scope = new Scope();
            foreach (var declarationNode in variableDeclarations)
            {
                var variableDeclarationNode =
                    VariableDeclarationNodeFactory.Create(declarationNode.name, declarationNode.expression);
                scope.AddOrUpdateVariableScopeEntry(variableDeclarationNode);
            }

            if (functionArguments is null)
            {
                return scope;
            }

            foreach (var functionArgument in functionArguments)
            {
                scope.AddVariableScopeEntry(functionArgument);
            }

            return scope;
        }
    }
}
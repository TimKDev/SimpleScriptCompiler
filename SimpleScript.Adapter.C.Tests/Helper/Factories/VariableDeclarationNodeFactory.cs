using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class VariableDeclarationNodeFactory
    {
        public static VariableDeclarationNode Create(string name, IExpression expression)
        {
            return new VariableDeclarationNode(name, expression);
        }
    }
}

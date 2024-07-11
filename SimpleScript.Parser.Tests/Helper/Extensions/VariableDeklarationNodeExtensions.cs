using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class VariableDeklarationNodeExtensions
    {
        public static void AssertWithInit(this VariableDeclarationNode variableDeclarationNode, string variableName)
        {
            variableDeclarationNode.VariableName.Should().Be(variableName);
        }

        public static TFirstChildNode AssertWithInit<TFirstChildNode>(this VariableDeclarationNode variableDeclarationNode, string variableName)
        {
            variableDeclarationNode.VariableName.Should().Be(variableName);
            variableDeclarationNode.InitialValue.Should().NotBeNull();
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(variableDeclarationNode.InitialValue!);
            return firstChild;
        }
    }
}

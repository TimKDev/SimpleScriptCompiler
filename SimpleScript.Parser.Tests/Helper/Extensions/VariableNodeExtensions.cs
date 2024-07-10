using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class VariableNodeExtensions
    {
        public static void Assert(this VariableNode variableNode, string variableName)
        {
            variableNode.Name.Should().Be(variableName);
        }
    }
}

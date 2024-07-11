using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class NumberNodeExtensions
    {
        public static void AssertValue(this NumberNode numberNode, int expectedValue)
        {
            numberNode.Value.Should().Be(expectedValue);
        }
    }
}

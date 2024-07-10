using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class StringNodeExtensions
    {
        public static void Assert(this StringNode stringNode, string expectedValue)
        {
            stringNode.Value.Should().Be(expectedValue);
        }
    }
}

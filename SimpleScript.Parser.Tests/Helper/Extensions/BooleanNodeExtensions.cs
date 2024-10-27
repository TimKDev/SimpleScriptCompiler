using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions;

internal static class BooleanNodeExtensions
{
    public static void AssertBoolean(this BooleanNode booleanNode, bool expectedValue)
    {
        booleanNode.Value.Should().Be(expectedValue);
    }
}
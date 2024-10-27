using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions;

internal static class SmallerOrEqualNodeExtensions
{
    public static (TFirstChildNode, TSecondChildNode) AssertSmallerOrEqual<TFirstChildNode, TSecondChildNode>(
        this SmallerOrEqualNode smallerOrEqualNode)
    {
        smallerOrEqualNode.FirstArgument.Should().NotBeNull();
        smallerOrEqualNode.SecondArgument.Should().NotBeNull();
        TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(smallerOrEqualNode.FirstArgument);
        TSecondChildNode secondChild = TH.ConvertTo<TSecondChildNode>(smallerOrEqualNode.SecondArgument);
        return (firstChild, secondChild);
    }
}
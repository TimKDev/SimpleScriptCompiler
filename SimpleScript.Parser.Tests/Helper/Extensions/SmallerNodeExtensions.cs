using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions;

internal static class SmallerNodeExtensions
{
    public static (TFirstChildNode, TSecondChildNode) AssertSmaller<TFirstChildNode, TSecondChildNode>(
        this SmallerNode smallerNode)
    {
        smallerNode.FirstArgument.Should().NotBeNull();
        smallerNode.SecondArgument.Should().NotBeNull();
        TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(smallerNode.FirstArgument);
        TSecondChildNode secondChild = TH.ConvertTo<TSecondChildNode>(smallerNode.SecondArgument);
        return (firstChild, secondChild);
    }
}
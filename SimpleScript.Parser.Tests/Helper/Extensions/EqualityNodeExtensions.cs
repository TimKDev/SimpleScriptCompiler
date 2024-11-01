using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions;

internal static class EqualityNodeExtensions
{
    public static (TFirstChildNode, TSecondChildNode) AssertEquality<TFirstChildNode, TSecondChildNode>(
        this EqualityNode equalityNode)
    {
        equalityNode.FirstArgument.Should().NotBeNull();
        equalityNode.SecondArgument.Should().NotBeNull();
        TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(equalityNode.FirstArgument);
        TSecondChildNode secondChild = TH.ConvertTo<TSecondChildNode>(equalityNode.SecondArgument);
        return (firstChild, secondChild);
    }
}
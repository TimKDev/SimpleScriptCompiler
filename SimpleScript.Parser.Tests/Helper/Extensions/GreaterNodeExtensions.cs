using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions;

internal static class GreaterNodeExtensions
{
    public static (TFirstChildNode, TSecondChildNode) AssertGreater<TFirstChildNode, TSecondChildNode>(
        this GreaterNode greaterNode)
    {
        greaterNode.FirstArgument.Should().NotBeNull();
        greaterNode.SecondArgument.Should().NotBeNull();
        TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(greaterNode.FirstArgument);
        TSecondChildNode secondChild = TH.ConvertTo<TSecondChildNode>(greaterNode.SecondArgument);
        return (firstChild, secondChild);
    }
}
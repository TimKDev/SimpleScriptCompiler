using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions;

internal static class GreaterOrEqualNodeExtensions
{
    public static (TFirstChildNode, TSecondChildNode) AssertGreaterOrEqual<TFirstChildNode, TSecondChildNode>(
        this GreaterOrEqualNode greaterOrEqualNode)
    {
        greaterOrEqualNode.FirstArgument.Should().NotBeNull();
        greaterOrEqualNode.SecondArgument.Should().NotBeNull();
        TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(greaterOrEqualNode.FirstArgument);
        TSecondChildNode secondChild = TH.ConvertTo<TSecondChildNode>(greaterOrEqualNode.SecondArgument);
        return (firstChild, secondChild);
    }
}
using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class MultiplyNodeExtensions
    {
        public static (TFirstChildNode, TSecondChildNode) AssertMultiplication<TFirstChildNode, TSecondChildNode>(this MultiplyNode multiplyNode)
        {
            multiplyNode.FirstArgument.Should().NotBeNull();
            multiplyNode.SecondArgument.Should().NotBeNull();
            TFirstChildNode? firstChild = TH.ConvertTo<TFirstChildNode>(multiplyNode.FirstArgument);
            TSecondChildNode? secondChild = TH.ConvertTo<TSecondChildNode>(multiplyNode.SecondArgument);
            return (firstChild, secondChild);
        }
    }
}

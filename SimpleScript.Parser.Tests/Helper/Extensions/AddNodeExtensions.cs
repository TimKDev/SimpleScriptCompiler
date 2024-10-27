using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class AddNodeExtensions
    {
        public static (TFirstChildNode, TSecondChildNode) AssertAddition<TFirstChildNode, TSecondChildNode>(
            this AddNode addNode)
        {
            addNode.FirstArgument.Should().NotBeNull();
            addNode.SecondArgument.Should().NotBeNull();
            TFirstChildNode? firstChild = TH.ConvertTo<TFirstChildNode>(addNode.FirstArgument);
            TSecondChildNode? secondChild = TH.ConvertTo<TSecondChildNode>(addNode.SecondArgument);
            return (firstChild, secondChild);
        }
    }
}
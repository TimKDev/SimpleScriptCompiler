using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class PrintNodeExtensions
    {
        public static TPrintExpression Assert<TPrintExpression>(this PrintNode printNode)
        {
            printNode.NodeToPrint.Should().NotBeNull();
            TPrintExpression firstChild = TH.ConvertTo<TPrintExpression>(printNode.NodeToPrint);
            return firstChild;
        }
    }
}

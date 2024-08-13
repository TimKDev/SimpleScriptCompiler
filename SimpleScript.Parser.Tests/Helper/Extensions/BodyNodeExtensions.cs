using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class BodyNodeExtensions
    {
        public static TStatement Assert<TStatement>(this BodyNode bodyNode)
        {
            bodyNode.ChildNodes.Count.Should().Be(1);
            return TH.ConvertTo<TStatement>(bodyNode.ChildNodes[0]);
        }

        public static (TFirstStatement, TSecondStatement) AssertBodyNode<TFirstStatement, TSecondStatement>(this BodyNode bodyNode)
        {
            bodyNode.ChildNodes.Count.Should().Be(2);
            return (TH.ConvertTo<TFirstStatement>(bodyNode.ChildNodes[0]), TH.ConvertTo<TSecondStatement>(bodyNode.ChildNodes[1]));
        }

        public static (TFirstStatement, TSecondStatement, TThirdStatement) Assert<TFirstStatement, TSecondStatement, TThirdStatement>(this BodyNode bodyNode)
        {
            bodyNode.ChildNodes.Count.Should().Be(3);
            return (TH.ConvertTo<TFirstStatement>(bodyNode.ChildNodes[0]), TH.ConvertTo<TSecondStatement>(bodyNode.ChildNodes[1]), TH.ConvertTo<TThirdStatement>(bodyNode.ChildNodes[2]));
        }
    }
}

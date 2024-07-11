using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class ProgramExtensions
    {
        public static TFirstChildNode Assert<TFirstChildNode>(this ProgramNode programNode)
        {
            programNode.ChildNodes.Count.Should().Be(1);
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(programNode.ChildNodes[0]);
            return firstChild;
        }

        public static (TFirstChildNode, TSecondChildNode) Assert<TFirstChildNode, TSecondChildNode>(this ProgramNode programNode)
        {
            programNode.ChildNodes.Count.Should().Be(2);
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(programNode.ChildNodes[0]);
            TSecondChildNode secondChild = TH.ConvertTo<TSecondChildNode>(programNode.ChildNodes[1]);
            return (firstChild, secondChild);
        }

        public static (TFirstChildNode, TSecondChildNode, TThirdChildNode) Assert<TFirstChildNode, TSecondChildNode, TThirdChildNode>(this ProgramNode programNode)
        {
            programNode.ChildNodes.Count.Should().Be(3);
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(programNode.ChildNodes[0]);
            TSecondChildNode secondChild = TH.ConvertTo<TSecondChildNode>(programNode.ChildNodes[1]);
            TThirdChildNode thirdChild = TH.ConvertTo<TThirdChildNode>(programNode.ChildNodes[2]);
            return (firstChild, secondChild, thirdChild);
        }
    }
}

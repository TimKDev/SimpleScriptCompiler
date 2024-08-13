using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class ProgramExtensions
    {
        public static TFirstChildNode AssertProgramNode<TFirstChildNode>(this ProgramNode programNode)
        {
            programNode.Body.ChildNodes.Count.Should().Be(1);
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(programNode.Body.ChildNodes[0]);
            return firstChild;
        }

        public static (TFirstChildNode, TSecondChildNode) Assert<TFirstChildNode, TSecondChildNode>(this ProgramNode programNode)
        {
            programNode.Body.ChildNodes.Count.Should().Be(2);
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(programNode.Body.ChildNodes[0]);
            TSecondChildNode secondChild = TH.ConvertTo<TSecondChildNode>(programNode.Body.ChildNodes[1]);
            return (firstChild, secondChild);
        }

        public static (TFirstChildNode, TSecondChildNode, TThirdChildNode) Assert<TFirstChildNode, TSecondChildNode, TThirdChildNode>(this ProgramNode programNode)
        {
            programNode.Body.ChildNodes.Count.Should().Be(3);
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(programNode.Body.ChildNodes[0]);
            TSecondChildNode secondChild = TH.ConvertTo<TSecondChildNode>(programNode.Body.ChildNodes[1]);
            TThirdChildNode thirdChild = TH.ConvertTo<TThirdChildNode>(programNode.Body.ChildNodes[2]);
            return (firstChild, secondChild, thirdChild);
        }
    }
}

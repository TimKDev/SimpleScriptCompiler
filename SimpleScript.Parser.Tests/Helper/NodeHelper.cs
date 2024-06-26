using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper
{
    public static class NodeHelper
    {
        public static TFirstChildNode AssertProgramNode<TFirstChildNode>(ProgramNode programNode)
        {
            programNode.ChildNodes.Count.Should().Be(1);
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(programNode.ChildNodes[0]);
            return firstChild;
        }

        public static TFirstChildNode AssertPrintNode<TFirstChildNode>(PrintNode printNode)
        {
            printNode.ChildNodes.Count.Should().Be(1);
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(printNode.ChildNodes[0]);
            return firstChild;
        }

        public static (TFirstChildNode, TSecondChildNode) AssertAddNode<TFirstChildNode, TSecondChildNode>(IExpression expression)
        {
            AddNode addNode = TH.ConvertTo<AddNode>(expression);
            addNode.ChildNodes.Count.Should().Be(2);
            TFirstChildNode? firstChild = TH.ConvertTo<TFirstChildNode>(addNode.ChildNodes[0]);
            TSecondChildNode? secondChild = TH.ConvertTo<TSecondChildNode>(addNode.ChildNodes[1]);
            return (firstChild, secondChild);
        }

        public static (TFirstChildNode, TSecondChildNode) AssertMultiplyNode<TFirstChildNode, TSecondChildNode>(IExpression expression)
        {
            MultiplyNode addNode = TH.ConvertTo<MultiplyNode>(expression);
            addNode.ChildNodes.Count.Should().Be(2);
            TFirstChildNode? firstChild = TH.ConvertTo<TFirstChildNode>(addNode.ChildNodes[0]);
            TSecondChildNode? secondChild = TH.ConvertTo<TSecondChildNode>(addNode.ChildNodes[1]);
            return (firstChild, secondChild);
        }
    }
}

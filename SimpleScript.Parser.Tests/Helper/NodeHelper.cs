using FluentAssertions;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Tests.Helper
{
    public static class NodeHelper
    {
        public static TFirstChildNode AssertPrintNode<TFirstChildNode>(PrintNode printNode)
        {
            printNode.NodeToPrint.Should().NotBeNull();
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(printNode.NodeToPrint);
            return firstChild;
        }

        public static TFirstChildNode AssertAssignVariableNode<TFirstChildNode>(VariableDeclarationNode variableDeclarationNode)
        {
            variableDeclarationNode.InitialValue.Should().NotBeNull();
            TFirstChildNode firstChild = TH.ConvertTo<TFirstChildNode>(variableDeclarationNode.InitialValue!);
            return firstChild;
        }

        public static (TFirstChildNode, TSecondChildNode) AssertAddNode<TFirstChildNode, TSecondChildNode>(IExpression expression)
        {
            AddNode addNode = TH.ConvertTo<AddNode>(expression);
            addNode.FirstArgument.Should().NotBeNull();
            addNode.SecondArgument.Should().NotBeNull();
            TFirstChildNode? firstChild = TH.ConvertTo<TFirstChildNode>(addNode.FirstArgument);
            TSecondChildNode? secondChild = TH.ConvertTo<TSecondChildNode>(addNode.SecondArgument);
            return (firstChild, secondChild);
        }

        public static (TFirstChildNode, TSecondChildNode) AssertMultiplyNode<TFirstChildNode, TSecondChildNode>(IExpression expression)
        {
            MultiplyNode multiplyNode = TH.ConvertTo<MultiplyNode>(expression);
            multiplyNode.FirstArgument.Should().NotBeNull();
            multiplyNode.SecondArgument.Should().NotBeNull();
            TFirstChildNode? firstChild = TH.ConvertTo<TFirstChildNode>(multiplyNode.FirstArgument);
            TSecondChildNode? secondChild = TH.ConvertTo<TSecondChildNode>(multiplyNode.SecondArgument);
            return (firstChild, secondChild);
        }
    }
}

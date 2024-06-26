using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;
using SimpleScript.Parser.Tests.Helper;
using SimpleScript.Parser.Tests.Helper.Factories;
using TF = SimpleScript.Parser.Tests.Helper.Factories.TokenFactory;
using TH = SimpleScript.Parser.Tests.Helper.TestHelper;

namespace SimpleScript.Parser.Tests.ComponentTests.ExpressionFactoryTests
{
    public class CreateExpressionTests
    {
        private readonly ExpressionFactory _sut = ExpressionFactoryFactory.Create();

        [Fact]
        public void ShouldCreateMulNodeWithTwoNumbers_GivenMultiplication()
        {
            List<Token> inputTokens = [TF.Num(2), TF.Mul(), TF.Num(5)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            MultiplyNode multiplyNode = TH.ConvertTo<MultiplyNode>(result);
            multiplyNode.ChildNodes.Count.Should().Be(2);
            NumberNode firstNumber = TH.ConvertTo<NumberNode>(multiplyNode.ChildNodes[0]);
            NumberNode secondNumber = TH.ConvertTo<NumberNode>(multiplyNode.ChildNodes[1]);
            firstNumber.Value.Should().Be(2);
            secondNumber.Value.Should().Be(5);
        }

        [Fact]
        public void ShouldCreateAddNodeWithVariableAndNumber_GivenTokens()
        {
            List<Token> inputTokens = [TF.Var("test"), TF.Add(), TF.Num(0)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            AddNode addNode = TH.ConvertTo<AddNode>(result);
            addNode.ChildNodes.Count.Should().Be(2);
            VariableNode variable = TH.ConvertTo<VariableNode>(addNode.ChildNodes[0]);
            NumberNode number = TH.ConvertTo<NumberNode>(addNode.ChildNodes[1]);
            variable.Name.Should().Be("test");
            number.Value.Should().Be(0);
        }

        [Fact]
        public void ShouldCreateMulNodeWithVariableAndNumber_GivenTokens()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Mul(), TF.Var("name")];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            MultiplyNode multiplyNode = TH.ConvertTo<MultiplyNode>(result);
            multiplyNode.ChildNodes.Count.Should().Be(2);
            NumberNode number = TH.ConvertTo<NumberNode>(multiplyNode.ChildNodes[0]);
            VariableNode variable = TH.ConvertTo<VariableNode>(multiplyNode.ChildNodes[1]);
            number.Value.Should().Be(43);
            variable.Name.Should().Be("name");
        }

        [Fact]
        public void ShouldCreateAddNode_GivenAddAndMulOperation()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Mul(), TF.Var("name"), TF.Add(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            result.Should().BeOfType<AddNode>();
        }

        [Fact]
        public void ShouldCreateAddNodeWithMulNodeAsChild_GivenAddAndMulOperation()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Mul(), TF.Var("name"), TF.Add(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            AddNode addNode = TH.ConvertTo<AddNode>(result);
            addNode.ChildNodes.Count.Should().Be(2);
            addNode.ChildNodes[0].Should().BeOfType<MultiplyNode>();
            addNode.ChildNodes[1].Should().BeOfType<NumberNode>();
        }

        [Fact]
        public void ShouldCreateMulNodeWithNumberAndVariableAsChild_GivenAddAndMulOperation()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Mul(), TF.Var("name"), TF.Add(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            AddNode addNode = TH.ConvertTo<AddNode>(result);
            MultiplyNode mulNode = TH.ConvertTo<MultiplyNode>(addNode.ChildNodes[0]);
            mulNode.ChildNodes.Count.Should().Be(2);
            NumberNode numberNode = TH.ConvertTo<NumberNode>(mulNode.ChildNodes[0]);
            VariableNode variableNode = TH.ConvertTo<VariableNode>(mulNode.ChildNodes[1]);
            numberNode.Value.Should().Be(43);
            variableNode.Name.Should().Be("name");
        }

        [Fact]
        public void ShouldCreateCorrectExpressionTree_Given2AddAndMulOperation()
        {
            List<Token> inputTokens = [TF.Num(1), TF.Add(), TF.Num(3), TF.Mul(), TF.Var("name"), TF.Add(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            (NumberNode? firstAddChild1, AddNode? firstAddChild2) = AssertAddNode<NumberNode, AddNode>(result);
            firstAddChild1.Value.Should().Be(1);
            (MultiplyNode? secondAddChild1, NumberNode? secondAddChild2) = AssertAddNode<MultiplyNode, NumberNode>(firstAddChild2);
            secondAddChild2.Value.Should().Be(2);
            (NumberNode? firstMulChild1, VariableNode? firstMulChild2) = AssertMultiplyNode<NumberNode, VariableNode>(secondAddChild1);
            firstMulChild1.Value.Should().Be(3);
            firstMulChild2.Name.Should().Be("name");
        }

        [Fact]
        public void ShouldCreateCorrectExpressionTree_Given2AddAnd2MulOperation()
        {
            List<Token> inputTokens = [TF.Num(1), TF.Mul(), TF.Num(2), TF.Mul(), TF.Num(3), TF.Add(), TF.Num(4), TF.Add(), TF.Num(5)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            (MultiplyNode? firstAddChild1, AddNode? firstAddChild2) = AssertAddNode<MultiplyNode, AddNode>(result);
            (NumberNode? firstMulChild1, MultiplyNode? firstMulChild2) = AssertMultiplyNode<NumberNode, MultiplyNode>(firstAddChild1);
            firstMulChild1.Value.Should().Be(1);
            (NumberNode? secondMulChild1, NumberNode? secondMulChild2) = AssertMultiplyNode<NumberNode, NumberNode>(firstMulChild2);
            secondMulChild1.Value.Should().Be(2);
            secondMulChild2.Value.Should().Be(3);
            (NumberNode? secondAddChild1, NumberNode? secondAddChild2) = AssertAddNode<NumberNode, NumberNode>(firstAddChild2);
            secondAddChild1.Value.Should().Be(4);
            secondAddChild2.Value.Should().Be(5);
        }

        private void AssertNumberNode(IAddable expression, int value)
        {
            NumberNode numberNode = TH.ConvertTo<NumberNode>(expression);
            numberNode.Value.Should().Be(value);
        }

        private (TFirstChildNode, TSecondChildNode) AssertAddNode<TFirstChildNode, TSecondChildNode>(IExpression expression)
        {
            AddNode addNode = TH.ConvertTo<AddNode>(expression);
            addNode.ChildNodes.Count.Should().Be(2);
            TFirstChildNode? firstChild = TH.ConvertTo<TFirstChildNode>(addNode.ChildNodes[0]);
            TSecondChildNode? secondChild = TH.ConvertTo<TSecondChildNode>(addNode.ChildNodes[1]);
            return (firstChild, secondChild);
        }

        private (TFirstChildNode, TSecondChildNode) AssertMultiplyNode<TFirstChildNode, TSecondChildNode>(IExpression expression)
        {
            MultiplyNode addNode = TH.ConvertTo<MultiplyNode>(expression);
            addNode.ChildNodes.Count.Should().Be(2);
            TFirstChildNode? firstChild = TH.ConvertTo<TFirstChildNode>(addNode.ChildNodes[0]);
            TSecondChildNode? secondChild = TH.ConvertTo<TSecondChildNode>(addNode.ChildNodes[1]);
            return (firstChild, secondChild);
        }
    }
}

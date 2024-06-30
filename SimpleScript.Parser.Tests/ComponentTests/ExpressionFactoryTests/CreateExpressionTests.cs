using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using SimpleScript.Parser.Tests.Helper.Factories;


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
            (NumberNode? firstNumber, NumberNode? secondNumber) = NH.AssertMultiplyNode<NumberNode, NumberNode>(multiplyNode);
            firstNumber.Value.Should().Be(2);
            secondNumber.Value.Should().Be(5);
        }

        [Fact]
        public void ShouldCreateAddNodeWithVariableAndNumber_GivenTokens()
        {
            List<Token> inputTokens = [TF.Var("test"), TF.Add(), TF.Num(0)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            AddNode addNode = TH.ConvertTo<AddNode>(result);
            (VariableNode? variable, NumberNode? number) = NH.AssertAddNode<VariableNode, NumberNode>(addNode);
            variable.Name.Should().Be("test");
            number.Value.Should().Be(0);
        }

        [Fact]
        public void ShouldCreateMulNodeWithVariableAndNumber_GivenTokens()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Mul(), TF.Var("name")];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            MultiplyNode multiplyNode = TH.ConvertTo<MultiplyNode>(result);
            (NumberNode? number, VariableNode? variable) = NH.AssertMultiplyNode<NumberNode, VariableNode>(multiplyNode);
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
            NH.AssertAddNode<MultiplyNode, NumberNode>(addNode);
        }

        [Fact]
        public void ShouldCreateMulNodeWithNumberAndVariableAsChild_GivenAddAndMulOperation()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Mul(), TF.Var("name"), TF.Add(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            (MultiplyNode? multiplyNode, NumberNode? addNumberNode) = NH.AssertAddNode<MultiplyNode, NumberNode>(result);
            addNumberNode.Value.Should().Be(2);
            (NumberNode? numberNode, VariableNode? variableNode) = NH.AssertMultiplyNode<NumberNode, VariableNode>(multiplyNode);
            numberNode.Value.Should().Be(43);
            variableNode.Name.Should().Be("name");
        }

        [Fact]
        public void ShouldCreateCorrectExpressionTree_Given2AddAndMulOperation()
        {
            List<Token> inputTokens = [TF.Num(1), TF.Add(), TF.Num(3), TF.Mul(), TF.Var("name"), TF.Add(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            (NumberNode? firstAddChild1, AddNode? firstAddChild2) = NH.AssertAddNode<NumberNode, AddNode>(result);
            firstAddChild1.Value.Should().Be(1);
            (MultiplyNode? secondAddChild1, NumberNode? secondAddChild2) = NH.AssertAddNode<MultiplyNode, NumberNode>(firstAddChild2);
            secondAddChild2.Value.Should().Be(2);
            (NumberNode? firstMulChild1, VariableNode? firstMulChild2) = NH.AssertMultiplyNode<NumberNode, VariableNode>(secondAddChild1);
            firstMulChild1.Value.Should().Be(3);
            firstMulChild2.Name.Should().Be("name");
        }

        [Fact]
        public void ShouldCreateCorrectExpressionTree_Given2AddAnd2MulOperation()
        {
            List<Token> inputTokens = [TF.Num(1), TF.Mul(), TF.Num(2), TF.Mul(), TF.Num(3), TF.Add(), TF.Num(4), TF.Add(), TF.Num(5)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            (MultiplyNode? firstAddChild1, AddNode? firstAddChild2) = NH.AssertAddNode<MultiplyNode, AddNode>(result);
            (NumberNode? firstMulChild1, MultiplyNode? firstMulChild2) = NH.AssertMultiplyNode<NumberNode, MultiplyNode>(firstAddChild1);
            firstMulChild1.Value.Should().Be(1);
            (NumberNode? secondMulChild1, NumberNode? secondMulChild2) = NH.AssertMultiplyNode<NumberNode, NumberNode>(firstMulChild2);
            secondMulChild1.Value.Should().Be(2);
            secondMulChild2.Value.Should().Be(3);
            (NumberNode? secondAddChild1, NumberNode? secondAddChild2) = NH.AssertAddNode<NumberNode, NumberNode>(firstAddChild2);
            secondAddChild1.Value.Should().Be(4);
            secondAddChild2.Value.Should().Be(5);
        }


    }
}

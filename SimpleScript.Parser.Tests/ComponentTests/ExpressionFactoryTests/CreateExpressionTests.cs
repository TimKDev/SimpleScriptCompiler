using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.Nodes;
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
    }
}

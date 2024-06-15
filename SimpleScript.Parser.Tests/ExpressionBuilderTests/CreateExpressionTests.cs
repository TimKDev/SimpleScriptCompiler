using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using TF = SimpleScript.Parser.Tests.Helper.TokenFactory;
using TH = SimpleScript.Parser.Tests.Helper.TestHelper;

namespace SimpleScript.Parser.Tests.ExpressionBuilderTests
{
    public class CreateExpressionTests
    {
        [Fact]
        public void ShouldCreateMulNodeWithTwoNumbers_GivenMultiplication()
        {
            List<Token> inputTokens = [TF.Num(2), TF.Mul(), TF.Num(5)];
            IExpression result = ErrorHelper.AssertResultSuccess(ExpressionFactory.Create(inputTokens));
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
            IExpression result = ErrorHelper.AssertResultSuccess(ExpressionFactory.Create(inputTokens));
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
            IExpression result = ErrorHelper.AssertResultSuccess(ExpressionFactory.Create(inputTokens));
            MultiplyNode multiplyNode = TH.ConvertTo<MultiplyNode>(result);
            multiplyNode.ChildNodes.Count.Should().Be(2);
            NumberNode number = TH.ConvertTo<NumberNode>(multiplyNode.ChildNodes[0]);
            VariableNode variable = TH.ConvertTo<VariableNode>(multiplyNode.ChildNodes[1]);
            number.Value.Should().Be(43);
            variable.Name.Should().Be("name");
        }

        [Fact]
        public void ShouldCreateAddNodeWithMulNodeAsChild_GivenAddAndMulOperation()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Mul(), TF.Var("name"), TF.Add(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(ExpressionFactory.Create(inputTokens));
            result.Should().BeOfType<AddNode>();
        }
    }
}

using FluentAssertions;
using NSubstitute;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;

namespace SimpleScript.Parser.Tests.UnitTests.ExpressionBuilderTests
{
    public class ExpressionFactoryTests
    {
        private readonly IAdditionNodeFactory _additionNodeFactory = new AdditionNodeFactory();
        private readonly IMultiplicationNodeFactory _multiplicationNodeFactory = new MultiplicationNodeFactory();
        private readonly ExpressionFactory _sut;

        public ExpressionFactoryTests()
        {
            _sut = new(_additionNodeFactory, _multiplicationNodeFactory);
        }


        [Fact]
        public void ShouldCreateNumberNode_GivenSingleNumberNode()
        {
            List<Token> inputTokens = [TF.Num(12)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            NumberNode numberNode = TestHelper.ConvertTo<NumberNode>(result);
            numberNode.Value.Should().Be(12);
        }

        [Fact]
        public void ShouldCreateStringNode_GivenSingleStringNode()
        {
            List<Token> inputTokens = [TF.Str("Hello")];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            StringNode stringNode = TestHelper.ConvertTo<StringNode>(result);
            stringNode.Value.Should().Be("Hello");
        }

        [Fact]
        public void ShouldCreateVariableNode_GivenSingleVariableNode()
        {
            List<Token> inputTokens = [TF.Var("name")];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            VariableNode variableNode = TestHelper.ConvertTo<VariableNode>(result);
            variableNode.Name.Should().Be("name");
        }

        [Fact]
        public void ShouldCreateAddNode_GivenAddAndMulOperation()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Mul(), TF.Var("name"), TF.Add(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            result.Should().BeOfType<AddNode>();
            List<Token> expectedFirstArgument = [TF.Num(43), TF.Mul(), TF.Var("name")];
            List<Token> expectedSecondArgument = [TF.Num(2)];
            AssertAddNodeFactoryCreateReceived(expectedFirstArgument, expectedSecondArgument);
        }

        [Fact]
        public void ShouldCreateAddNode_GivenMulAndAddOperation()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Add(), TF.Var("name"), TF.Mul(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            result.Should().BeOfType<AddNode>();
            List<Token> expectedFirstArgument = [TF.Num(43)];
            List<Token> expectedSecondArgument = [TF.Var("name"), TF.Mul(), TF.Num(2)];
            AssertAddNodeFactoryCreateReceived(expectedFirstArgument, expectedSecondArgument);
        }

        private void AssertAddNodeFactoryCreateReceived(List<Token> firstArg, List<Token> secondArg)
        {
            _additionNodeFactory.Received(1).Create(
                Arg.Is<List<Token>>(arg => firstArg.SequenceEqual(arg, new TokenComparer())),
                Arg.Is<List<Token>>(arg => secondArg.SequenceEqual(arg, new TokenComparer())),
                Arg.Any<IExpressionFactory>()
            );
        }
    }
}

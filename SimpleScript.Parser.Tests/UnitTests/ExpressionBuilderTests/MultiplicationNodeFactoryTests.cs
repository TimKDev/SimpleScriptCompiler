using FluentAssertions;
using NSubstitute;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using TF = SimpleScript.Parser.Tests.Helper.Factories.TokenFactory;

namespace SimpleScript.Parser.Tests.UnitTests.ExpressionBuilderTests
{
    public class MultiplicationNodeFactoryTests
    {
        private readonly IExpressionFactory _expressionFactory = Substitute.For<IExpressionFactory>();
        private readonly MultiplicationNodeFactory _sut = new();

        [Fact]
        public void ShouldCreateOneAddNodeWithTwoNumberChildNodes_GivenTwoNumbersAsArgs()
        {
            List<Token> firstArg = [TF.Num(43)];
            List<Token> secondArg = [TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(firstArg, secondArg, _expressionFactory));
            MultiplyNode multiplyNode = TestHelper.ConvertTo<MultiplyNode>(result);
            multiplyNode.ChildNodes.Count.Should().Be(2);
            NumberNode firstChild = TestHelper.ConvertTo<NumberNode>(multiplyNode.ChildNodes[0]);
            NumberNode secondChild = TestHelper.ConvertTo<NumberNode>(multiplyNode.ChildNodes[1]);
            firstChild.Value.Should().Be(43);
            secondChild.Value.Should().Be(2);
            _expressionFactory.Received(0).Create(Arg.Any<List<Token>>());
        }

        [Fact]
        public void ShouldCallExpressionNodeFactory_GivenFirstArgWithMulOperation()
        {
            _expressionFactory.Create(Arg.Any<List<Token>>()).Returns(new MultiplyNode());
            List<Token> firstArg = [TF.Num(43), TF.Mul(), TF.Var("i")];
            List<Token> secondArg = [TF.Num(2)];
            ErrorHelper.AssertResultSuccess(_sut.Create(firstArg, secondArg, _expressionFactory));
            AssertExpressionFactoryCreateReceived(firstArg);
        }

        [Fact]
        public void ShouldCallExpressionNodeFactoryTwoTimes_GivenBothArgWithBinaryOperation()
        {
            _expressionFactory.Create(Arg.Any<List<Token>>()).Returns(new MultiplyNode());
            List<Token> firstArg = [TF.Num(1), TF.Add(), TF.Var("i")];
            List<Token> secondArg = [TF.Str("Hello World"), TF.Mul(), TF.Var("i")];
            ErrorHelper.AssertResultSuccess(_sut.Create(firstArg, secondArg, _expressionFactory));
            AssertExpressionFactoryCreateReceived(firstArg);
            AssertExpressionFactoryCreateReceived(secondArg);
        }

        [Fact]
        public void ShouldReturnNotSupportedError_GivenString()
        {
            List<Token> firstArg = [TF.Num(1)];
            List<Token> secondArg = [TF.Str("Hello World")];
            EntertainingErrors.Result<MultiplyNode> result = _sut.Create(firstArg, secondArg, _expressionFactory);
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count().Should().Be(1);
            result.Errors[0].Message.Should().Be("Error Line 1: Token type String is not supported for multiplication.");
        }

        private void AssertExpressionFactoryCreateReceived(List<Token> tokens)
        {
            _expressionFactory.Received(1).Create(
                Arg.Is<List<Token>>(arg => tokens.SequenceEqual(arg, new TokenComparer()))
            );
        }
    }
}

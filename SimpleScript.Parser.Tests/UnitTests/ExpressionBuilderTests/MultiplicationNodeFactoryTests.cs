using EntertainingErrors;
using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ExpressionBuilderTests
{
    public class MultiplicationNodeFactoryTests
    {
        private readonly IExpressionFactory _expressionFactory = ExpressionFactoryFactory.Create();
        private readonly MultiplicationNodeFactory _sut = new();

        [Fact]
        public void ShouldCreateOneAddNodeWithTwoNumberChildNodes_GivenTwoNumbersAsArgs()
        {
            List<Token> firstArg = [TF.Num(43)];
            List<Token> secondArg = [TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(firstArg, secondArg, _expressionFactory));
            MultiplyNode multiplyNode = TestHelper.ConvertTo<MultiplyNode>(result);
            (NumberNode firstChild, NumberNode secondChild) = NH.AssertMultiplyNode<NumberNode, NumberNode>(multiplyNode);
            firstChild.Value.Should().Be(43);
            secondChild.Value.Should().Be(2);
        }

        [Fact]
        public void ShouldReturnNotSupportedError_GivenString()
        {
            List<Token> firstArg = [TF.Num(1)];
            List<Token> secondArg = [TF.Str("Hello World")];
            Result<MultiplyNode> result = _sut.Create(firstArg, secondArg, _expressionFactory);
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count().Should().Be(1);
            result.Errors[0].Message.Should().Be("Error Line 1: Expression is not supported for multiplication.");
        }
    }
}

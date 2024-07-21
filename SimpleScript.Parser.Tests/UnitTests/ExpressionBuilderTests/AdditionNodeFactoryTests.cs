using NSubstitute;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;

namespace SimpleScript.Parser.Tests.UnitTests.ExpressionBuilderTests
{
    public class AdditionNodeFactoryTests
    {
        private readonly IExpressionFactory _expressionFactory = Substitute.For<IExpressionFactory>();
        private readonly AdditionNodeFactory _sut = new();

        [Fact]
        public void ShouldCallExpressionNodeFactory_GivenFirstArgWithMulOperation()
        {
            _expressionFactory.Create(Arg.Any<List<Token>>()).Returns(MultiplyNode.Create(new NumberNode(43, 1, 1), new VariableNode("i", 1, 1)).Value);
            List<Token> firstArg = [TF.Num(43), TF.Mul(), TF.Var("i")];
            List<Token> secondArg = [TF.Num(2)];
            ErrorHelper.AssertResultSuccess(_sut.Create(firstArg, secondArg, _expressionFactory));
            AssertExpressionFactoryCreateReceived(firstArg);
        }

        [Fact]
        public void ShouldCallExpressionNodeFactoryTwoTimes_GivenBothArgWithBinaryOperation()
        {
            _expressionFactory.Create(Arg.Any<List<Token>>()).Returns(MultiplyNode.Create(new NumberNode(1, 1, 1), new VariableNode("i", 1, 1)).Value);
            List<Token> firstArg = [TF.Num(1), TF.Add(), TF.Var("i")];
            List<Token> secondArg = [TF.Str("Hello World"), TF.Mul(), TF.Var("i")];
            ErrorHelper.AssertResultSuccess(_sut.Create(firstArg, secondArg, _expressionFactory));
            AssertExpressionFactoryCreateReceived(firstArg);
            AssertExpressionFactoryCreateReceived(secondArg);
        }

        private void AssertExpressionFactoryCreateReceived(List<Token> tokens)
        {
            _expressionFactory.Received(1).Create(
                Arg.Is<List<Token>>(arg => tokens.SequenceEqual(arg, new TokenComparer()))
            );
        }
    }
}

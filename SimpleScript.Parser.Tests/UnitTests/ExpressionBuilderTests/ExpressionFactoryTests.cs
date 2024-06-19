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
    public class ExpressionFactoryTests
    {
        private readonly IAdditionNodeFactory _additionNodeFactory = Substitute.For<IAdditionNodeFactory>();
        private readonly IMultiplicationNodeFactory _multiplicationNodeFactory = Substitute.For<IMultiplicationNodeFactory>();
        private readonly ExpressionFactory _sut;

        public ExpressionFactoryTests()
        {
            _additionNodeFactory.Create(Arg.Any<List<Token>>(), Arg.Any<List<Token>>()).Returns(new AddNode());
            _sut = new(_additionNodeFactory, _multiplicationNodeFactory);
        }

        [Fact]
        public void ShouldCreateMulNodeWithNumberAndVariableAsChild_GivenAddAndMulOperation()
        {
            List<Token> inputTokens = [TF.Num(43), TF.Mul(), TF.Var("name"), TF.Add(), TF.Num(2)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            result.Should().BeOfType<AddNode>();
            List<Token> expectedFirstArgument = [TF.Num(43), TF.Mul(), TF.Var("name")];
            List<Token> expectedSecondArgument = [TF.Num(2)];

            _additionNodeFactory.Received(1).Create(
                Arg.Is<List<Token>>(firstArg => expectedFirstArgument.SequenceEqual(firstArg, new TokenComparer())),
                Arg.Is<List<Token>>(secondArg => expectedSecondArgument.SequenceEqual(secondArg, new TokenComparer()))
            );
        }

        [Fact]
        public void ShouldCreateNumberNode_GivenSingleNumberNode()
        {
            List<Token> inputTokens = [TF.Num(12)];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            NumberNode numberNode = TestHelper.ConvertTo<NumberNode>(result);
            numberNode.Value.Should().Be(12);
        }
    }
}

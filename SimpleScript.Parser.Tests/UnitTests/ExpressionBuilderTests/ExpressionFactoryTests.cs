using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;
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
    }
}

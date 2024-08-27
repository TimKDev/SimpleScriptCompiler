using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ExpressionBuilderTests
{
    public class ExpressionFactoryTests
    {
        private readonly ExpressionFactory _sut = ExpressionFactoryFactory.Create();

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

        //More complex operations with Brackets!

        [Fact]
        public void ShouldCreateFunctionInvocation_GivenSingleFunctionCallWithoutArguments()
        {
            List<Token> inputTokens = [TF.Var("getName"), TF.Open(), TF.Close()];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            FunctionInvocationNode functionInvocation = TestHelper.ConvertTo<FunctionInvocationNode>(result);
            functionInvocation.FunctionName.Should().Be("getName");
        }

        [Fact]
        public void ShouldCreateFunctionInvocation_GivenSingleFunctionCallWithOneArgument()
        {
            List<Token> inputTokens = [TF.Var("doubleNumber"), TF.Open(), TF.Num(5), TF.Close()];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            FunctionInvocationNode functionInvocation = TestHelper.ConvertTo<FunctionInvocationNode>(result);
            NumberNode numberNode = functionInvocation.Assert<NumberNode>("doubleNumber");
            numberNode.Assert(5);
        }

        [Fact]
        public void ShouldCreateFunctionInvocation_GivenSingleFunctionCallWithOneExpressionArgument()
        {
            List<Token> inputTokens = [TF.Var("doubleNumber"), TF.Open(), TF.Num(5), TF.Add(), TF.Num(6), TF.Close()];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            FunctionInvocationNode functionInvocation = TestHelper.ConvertTo<FunctionInvocationNode>(result);
            AddNode addNode = functionInvocation.Assert<AddNode>("doubleNumber");
            (NumberNode numberNode1, NumberNode numberNode2) = addNode.Assert<NumberNode, NumberNode>();
            numberNode1.Assert(5);
            numberNode2.Assert(6);
        }

        [Fact]
        public void ShouldCreateFunctionInvocation_GivenSingleFunctionCallWithTwoArguments()
        {
            List<Token> inputTokens = [TF.Var("getName"), TF.Open(), TF.Close()];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            FunctionInvocationNode functionInvocation = TestHelper.ConvertTo<FunctionInvocationNode>(result);
            functionInvocation.FunctionName.Should().Be("getName");
        }

        [Fact]
        public void ShouldCreateFunctionInvocation_GivenSingleFunctionCallWithComplexExpressionArgument()
        {
            List<Token> inputTokens = [TF.Var("doubleNumber"), TF.Open(), TF.Num(5), TF.Add(), TF.Num(6), TF.Mul(), TF.Num(7), TF.Close()];
            IExpression result = ErrorHelper.AssertResultSuccess(_sut.Create(inputTokens));
            FunctionInvocationNode functionInvocation = TestHelper.ConvertTo<FunctionInvocationNode>(result);
            AddNode addNode = functionInvocation.Assert<AddNode>("doubleNumber");
            (NumberNode numberNode, MultiplyNode multiplyNode) = addNode.Assert<NumberNode, MultiplyNode>();
            numberNode.Assert(5);
            (NumberNode numberNode1, NumberNode numberNode2) = multiplyNode.Assert<NumberNode, NumberNode>();
            numberNode1.Assert(6);
            numberNode2.Assert(7);
        }
    }
}

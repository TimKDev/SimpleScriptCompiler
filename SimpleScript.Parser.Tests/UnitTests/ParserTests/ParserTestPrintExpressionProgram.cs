using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParserTestPrintExpressionProgram
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParseTokens_ShouldGeneratePrintNodeWithMultiplyNode_GivenProgramTokens()
        {
            List<Token> programTokens = [TF.Print(), TF.Num(1), TF.Mul(), TF.Num(2)];
            ProgramNode result = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            PrintNode printNode = NH.AssertProgramNode<PrintNode>(result);
            MultiplyNode multiplyNode = NH.AssertPrintNode<MultiplyNode>(printNode);
            (NumberNode? num1, NumberNode? num2) = NH.AssertMultiplyNode<NumberNode, NumberNode>(multiplyNode);
            num1.Value.Should().Be(1);
            num2.Value.Should().Be(2);
        }

        [Fact]
        public void ParseTokens_ShouldGeneratePrintNodeWithAddNode_GivenProgramTokens()
        {
            List<Token> programTokens = [TF.Print(), TF.Num(1), TF.Add(), TF.Num(2), TF.Mul(), TF.Var("test123")];
            ProgramNode result = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            PrintNode printNode = NH.AssertProgramNode<PrintNode>(result);
            AddNode addNode = NH.AssertPrintNode<AddNode>(printNode);
            (NumberNode? num1, MultiplyNode? mulNode) = NH.AssertAddNode<NumberNode, MultiplyNode>(addNode);
            num1.Value.Should().Be(1);
            (NumberNode? mulNum1, VariableNode? mulNum2) = NH.AssertMultiplyNode<NumberNode, VariableNode>(mulNode);
            mulNum1.Value.Should().Be(2);
            mulNum2.Name.Should().Be("test123");
        }
    }
}

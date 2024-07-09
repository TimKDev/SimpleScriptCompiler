using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using SimpleScript.Parser.Tests.Helper.Factories;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParserTestsPrintNumbersWithPlusProgram
    {
        private static readonly int num1 = 1;
        private static readonly int num2 = 2;
        private readonly List<Token> ProgramTokens = [TF.Print(), TF.Num(num1), TF.Add(), TF.Num(num2)];
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParserTests_PrintNodeShouldHaveAddNodeAsChild_GivenProgramTokens()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = NH.AssertProgramNode<PrintNode>(programmingNode);
            NH.AssertPrintNode<AddNode>(printNode);
        }

        [Fact]
        public void ParserTests_AddNodeShouldHaveTwoChildNodes()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = NH.AssertProgramNode<PrintNode>(programmingNode);
            AddNode addNode = NH.AssertPrintNode<AddNode>(printNode);
            addNode.FirstArgument.Should().NotBeNull();
        }

        [Fact]
        public void ParserTests_NumbersShouldHaveValues1And2()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = NH.AssertProgramNode<PrintNode>(programmingNode);
            AddNode addNode = NH.AssertPrintNode<AddNode>(printNode);
            (NumberNode firstNumber, NumberNode secondNumber) = NH.AssertAddNode<NumberNode, NumberNode>(addNode);
            firstNumber.Value.Should().Be(num1);
            secondNumber.Value.Should().Be(num2);
        }
    }
}

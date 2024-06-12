using FluentAssertions;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using SimpleScriptCompiler.LexicalAnalysis;
using TF = SimpleScript.Parser.Tests.Helper.TokenFactory;

namespace SimpleScript.Parser.Tests.ParserTests
{
    public class ParserTestsPrintNumbersWithPlusProgram
    {
        private readonly Parser _sut = new();
        private static readonly int num1 = 1;
        private static readonly int num2 = 2;
        private readonly List<Token> ProgramTokens = [TF.Print(), TF.Num(num1), TF.Add(), TF.Num(num2)];

        [Fact]
        public void ParserTests_PrintNodeShouldHaveAddNodeAsChild_GivenProgramTokens()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            programmingNode.ChildNodes.Count.Should().Be(1);
            programmingNode.ChildNodes[0].Should().BeOfType<PrintNode>();
            PrintNode printNode = programmingNode.ChildNodes[0];
            printNode.ChildNodes.Count().Should().Be(1);
            printNode.ChildNodes[0].Should().BeOfType<AddNode>();
        }

        [Fact]
        public void ParserTests_AddNodeShouldHaveTwoChildNodes()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = programmingNode.ChildNodes[0];
            AddNode? addNode = printNode.ChildNodes[0] as AddNode;
            addNode!.ChildNodes.Count.Should().Be(2);
        }

        [Fact]
        public void ParserTests_NumbersShouldHaveValues1And2()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = programmingNode.ChildNodes[0];
            AddNode addNode = (printNode.ChildNodes[0] as AddNode)!;
            NumberNode? firstNumber = addNode.ChildNodes[0] as NumberNode;
            NumberNode? secondNumber = addNode.ChildNodes[1] as NumberNode;
            firstNumber!.Value.Should().Be(num1);
            secondNumber!.Value.Should().Be(num2);
        }
    }
}

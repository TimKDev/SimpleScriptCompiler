using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using SimpleScript.Parser.Tests.Helper.Factories;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParserTestsHelloSimpleScriptProgram
    {
        private static readonly string HelloMessage = "Hello SimpleScript";
        private readonly List<Token> ProgramTokens = [TF.Print(), TF.Str(HelloMessage)];

        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParseTokens_ShouldParseReturnProgramNode_GivenProgramTokens()
        {
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            programNode.Should().BeOfType<ProgramNode>();
        }

        [Fact]
        public void ParseTokens_ShouldReturnProgramNodeWithPrintChild_GivenProgramTokens()
        {
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            programNode.ChildNodes.Count.Should().Be(1);
            programNode.ChildNodes[0].Should().BeOfType<PrintNode>();
        }

        [Fact]
        public void ParseTokens_PrintNodeShouldHaveStringNodeChild_GivenProgramTokens()
        {
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = programNode.ChildNodes[0] as PrintNode;
            printNode.ChildNodes.Count.Should().Be(1);
            printNode.ChildNodes[0].Should().BeOfType<StringNode>();
        }

        [Fact]
        public void ParseTokens_StringNodeShouldHaveValueHelloSimpleScript_GivenProgramTokens()
        {
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = programNode.ChildNodes[0] as PrintNode;
            StringNode? stringNode = printNode.ChildNodes[0] as StringNode;
            stringNode!.Value.Should().Be(HelloMessage);
        }
    }
}
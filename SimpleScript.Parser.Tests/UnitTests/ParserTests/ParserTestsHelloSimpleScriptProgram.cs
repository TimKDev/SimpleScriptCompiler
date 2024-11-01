using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

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
            programNode.AssertProgramNode<PrintNode>();
        }

        [Fact]
        public void ParseTokens_PrintNodeShouldHaveStringNodeChild_GivenProgramTokens()
        {
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            programNode
                .AssertProgramNode<PrintNode>()
                .AssertPrint<StringNode>();
        }

        [Fact]
        public void ParseTokens_StringNodeShouldHaveValueHelloSimpleScript_GivenProgramTokens()
        {
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            programNode
                .AssertProgramNode<PrintNode>()
                .AssertPrint<StringNode>()
                .AssertString(HelloMessage);
        }
    }
}
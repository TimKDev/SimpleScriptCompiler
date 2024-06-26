using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using SimpleScript.Parser.Tests.Helper.Factories;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParserTestHelloWorldProgram
    {
        private static readonly string HelloMessage = "Hello World";
        private readonly List<Token> ProgramTokens = [TF.Print(), TF.Str(HelloMessage)];
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParseTokens_StringNodeShouldHaveValueHelloSimpleScript_GivenProgramTokens()
        {
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode? printNode = programNode.ChildNodes[0] as PrintNode;
            StringNode? stringNode = printNode.ChildNodes[0] as StringNode;
            stringNode!.Value.Should().Be(HelloMessage);
        }
    }
}

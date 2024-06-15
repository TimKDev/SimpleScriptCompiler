using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using TF = SimpleScript.Parser.Tests.Helper.TokenFactory;

namespace SimpleScript.Parser.Tests.ParserTests
{
    public class ParserTestHelloWorldProgram
    {
        private readonly Parser _sut = new();
        private static readonly string HelloMessage = "Hello World";
        private readonly List<Token> ProgramTokens = [TF.Print(), TF.Str(HelloMessage)];

        [Fact]
        public void ParseTokens_StringNodeShouldHaveValueHelloSimpleScript_GivenProgramTokens()
        {
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = programNode.ChildNodes[0];
            StringNode? stringNode = printNode.ChildNodes[0] as StringNode;
            stringNode!.Value.Should().Be(HelloMessage);
        }
    }
}

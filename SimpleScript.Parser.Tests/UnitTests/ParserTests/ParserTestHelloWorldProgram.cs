using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

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
            programNode
                .AssertProgramNode<PrintNode>()
                .AssertPrint<StringNode>()
                .AssertString(HelloMessage);
        }
    }
}

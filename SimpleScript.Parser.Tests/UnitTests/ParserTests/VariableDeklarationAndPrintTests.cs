using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using SimpleScript.Parser.Tests.Helper.Factories;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class VariableDeklarationAndPrintTests
    {
        private readonly Parser _sut = ParserFactory.Create();
        private readonly List<Token> programTokens = [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Str("Hello World"), TF.Print(), TF.Var("hello")];

        [Fact]
        public void ShouldCreateAProgramNodeWithTwoChildNodes()
        {
            ProgramNode result = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            result.ChildNodes.Should().HaveCount(2);
        }
    }
}

using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParseInputVariable
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParseTokens_ShouldCreateInputNodeWithVariableName_GivenInput()
        {
            List<Token> programTokens = [TF.Input(), TF.Var("name")];
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            InputNode inputNode = NH.AssertProgramNode<InputNode>(programNode);
            inputNode.VariableName.Should().Be("name");
        }
    }
}

using FluentAssertions;
using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes;

namespace SimpleScriptCompiler.Tests
{
    public class ParserTests
    {
        private readonly Parser _sut = new();

        [Fact]
        public void ParseToAST_ProgramNodeShouldHaveChildNode_GivenVariableDeklarartion()
        {
            var tokens = new List<Token>()
            {
                new Token(TokenType.LET, 1),
                new Token(TokenType.Variable, 1, "test"),
                new Token(TokenType.ASSIGN, 1),
                new Token(TokenType.Number, 1, "42")
            };

            var result = _sut.ParseToAST(tokens);
            result.ChildNodes.Should().HaveCount(1);
            result.ChildNodes[0].Should().BeOfType<VariableDeklarationNode>();
        }
    }
}

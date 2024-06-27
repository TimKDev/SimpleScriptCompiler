using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using SimpleScript.Parser.Tests.Helper.Factories;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParserTestsWithVariableDeklaration
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParseTokens_ShouldGenerateVariableDeklarationNode_GivenProgramTokens()
        {
            string variableName = "name";
            List<Token> programTokens = [TF.Let(), TF.Var(variableName)];
            ProgramNode result = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            VariableDeclarationNode variableDeklarationNode = NH.AssertProgramNode<VariableDeclarationNode>(result);
            variableDeklarationNode.VariableName.Should().Be(variableName);
        }

        [Fact]
        public void ParseTokens_ShouldGenerateError_GivenLetWithoutAnyThing()
        {
            List<Token> programTokens = [TF.Let()];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage("Invalid usage of Let keyword. Let should be followed by a variable name.", 1)]);
        }

        [Fact]
        public void ParseTokens_ShouldGenerateError_GivenLetWithoutVariable()
        {
            List<Token> programTokens = [TF.Let(), TF.Num(33)];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage("Invalid usage of Let keyword. Let should be followed by a variable name.", 1)]);
        }

        [Fact]
        public void ParseTokens_ShouldGenerateError_GivenLetWithVariableNameEqualsNull()
        {
            List<Token> programTokens = [TF.Let(), TF.Var(null)];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage("Invalid usage of Let keyword. Let should be followed by a variable name not equals to null.", 1)]);
        }
    }
}

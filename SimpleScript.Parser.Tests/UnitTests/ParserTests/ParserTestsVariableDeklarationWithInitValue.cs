using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using SimpleScript.Parser.Tests.Helper.Factories;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParserTestsVariableDeklarationWithInitValue
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParseTokens_ShouldGenerateVariableDeklarationNodeWithInitValue_GivenProgramTokens()
        {
            string variableName = "name";
            string variableValue = "Tim";
            List<Token> programTokens = [TF.Let(), TF.Var(variableName), TF.Assign(), TF.Str(variableValue)];
            ProgramNode result = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            VariableDeclarationNode variableDeklarationNode = NH.AssertProgramNode<VariableDeclarationNode>(result);
            variableDeklarationNode.VariableName.Should().Be(variableName);
            StringNode initValue = NH.AssertAssignVariableNode<StringNode>(variableDeklarationNode);
            initValue.Value.Should().Be(variableValue);
        }
    }
}

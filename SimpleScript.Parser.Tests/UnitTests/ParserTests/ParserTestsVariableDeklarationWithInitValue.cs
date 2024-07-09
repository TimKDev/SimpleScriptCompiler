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

        [Fact]
        public void ParseTokens_ShouldGenerateVariableDeklarationNodeWithExpressionAsInitValue_GivenProgramTokens()
        {
            string variableName = "name";
            List<Token> programTokens = [TF.Let(), TF.Var(variableName), TF.Assign(), TF.Num(3), TF.Add(), TF.Num(4)];
            ProgramNode result = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            VariableDeclarationNode variableDeklarationNode = NH.AssertProgramNode<VariableDeclarationNode>(result);
            variableDeklarationNode.VariableName.Should().Be(variableName);
            AddNode initValue = NH.AssertAssignVariableNode<AddNode>(variableDeklarationNode);
            (NumberNode num1, NumberNode num2) = NH.AssertAddNode<NumberNode, NumberNode>(initValue);
            num1.Value.Should().Be(3);
            num2.Value.Should().Be(4);
        }

        [Fact]
        public void ParseTokens_ShouldCreateError_WhenNoValueFollowsAfterAssert()
        {
            List<Token> programTokens = [TF.Let(), TF.Var(""), TF.Assign()];
            string[] expectedErrorMessages = [ErrorHelper.CreateErrorMessage("Missing Assert Value: No value given after the assert symbol.", 1)];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), expectedErrorMessages);
        }

        [Fact]
        public void ParseTokens_ShouldCreateError_WhenGivenInvalidExpression()
        {
            List<Token> programTokens = [TF.Let(), TF.Var(""), TF.Assign(), TF.Add(), TF.Num(23)];
            string[] expectedErrorMessages = [ErrorHelper.CreateErrorMessage($"Invalid Expression: {ErrorHelper.CreateErrorMessage("Binary Operation is missing operant.", 1)}", 1)];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), expectedErrorMessages);
        }
    }
}

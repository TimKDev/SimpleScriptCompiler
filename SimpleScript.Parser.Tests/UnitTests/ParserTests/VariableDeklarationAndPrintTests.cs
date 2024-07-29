using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class VariableDeklarationAndPrintTests
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ShouldCreateAProgramNodeWithTwoChildNodes()
        {
            List<Token> programTokens = [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Str("Hello World"), TF.Print(), TF.Var("hello")];
            ProgramNode result = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            result.ChildNodes.Should().HaveCount(2);
        }

        [Fact]
        public void ShouldCreateVariableWithCorrectValue_And_PrintOfThisVariable()
        {
            List<Token> programTokens = [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Str("Hello World"), TF.Print(), TF.Var("hello")];
            ProgramNode result = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            (VariableDeclarationNode variableDeklarationNode, PrintNode printNode) = NH.AssertProgramNode<VariableDeclarationNode, PrintNode>(result);
            StringNode stringNode = NH.AssertAssignVariableNode<StringNode>(variableDeklarationNode);
            VariableNode variableToPrint = NH.AssertPrintNode<VariableNode>(printNode);
            variableDeklarationNode.VariableName.Should().Be("hello");
            stringNode.Assert("Hello World");
            variableToPrint.Assert("hello");
        }

        [Fact]
        public void ShouldReturnError_WhenNumberAndNoVarIsGivenInVariableDeklaration()
        {
            List<Token> programTokens = [TF.Let(), TF.Num(2), TF.Assign(), TF.Str("Hello World"), TF.Print(), TF.Var("hello")];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage("Invalid usage of Let keyword. Let should be followed by a variable name and an initial value.", 1)]);
        }

        [Fact]
        public void ShouldReturnError_WhenVariableDeklarationHasInvalidExpression()
        {
            List<Token> programTokens = [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Add(), TF.Print(), TF.Var("hello")];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage($"Invalid Expression: {ErrorHelper.CreateErrorMessage("Binary Operation is missing operant.", 1)}", 1)]);
        }

        [Fact]
        public void ShouldReturnError_WhenPrintArgumentIsMissing()
        {
            List<Token> programTokens = [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Str("Hello World"), TF.Print()];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage($"Invalid usage of Print keyword. Print should be followed by expression to print.", 1)]);
        }

        [Fact]
        public void ShouldReturnError_WhenVariableDeklarationHasInvalidExpression_And_WhenPrintArgumentIsMissing()
        {
            List<Token> programTokens = [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Add(), TF.Print()];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [
                ErrorHelper.CreateErrorMessage($"Invalid usage of Print keyword. Print should be followed by expression to print.", 1),
                ErrorHelper.CreateErrorMessage($"Invalid Expression: {ErrorHelper.CreateErrorMessage("Binary Operation is missing operant.", 1)}", 1)
            ]);
        }
    }
}

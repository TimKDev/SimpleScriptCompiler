using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Tests.Helper;

namespace SimpleScript.Parser.Tests.UnitTests
{
    public class StatementCombinerTests
    {
        private readonly StatementCombiner _sut = new();

        [Fact]
        public void ShouldCreateTwoStatements_GivenAssignmentAndPrint()
        {
            List<Token> tokens = [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Str("Hello World"), TF.Print(), TF.Var("hello")];
            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
        }

        [Fact]
        public void ShouldCreateTwoStatementsWithTokens_GivenAssignmentAndPrint()
        {
            List<Token> tokens = [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Str("Hello World"), TF.Print(), TF.Var("hello")];
            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
            Statement variableAssignmentStatement = statements[0];
            Statement printStatement = statements[1];
            variableAssignmentStatement.Tokens.Count.Should().Be(4);
            printStatement.Tokens.Count().Should().Be(2);
        }
    }
}

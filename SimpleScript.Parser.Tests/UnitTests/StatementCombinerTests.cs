using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Tests.Shared;

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

        [Fact]
        public void ShouldCreateTwoStatementsInRoot_GivenFunctionDefinitionAndPrint()
        {
            //FUNC add(int num_1, int num_2)
            //BODY
            //    LET result = num_1 + num_2
            //    RETURN result
            //ENDBODY
            //PRINT add(23, 55)
            List<Token> tokens = [TF.Func(), TF.Var("add"), TF.Open(), TF.Int(), TF.Var("num_1"), TF.Comma(), TF.Int(), TF.Var("num_2"), TF.Close(), TF.Body(), TF.Let(), TF.Var("result"), TF.Assign(), TF.Var("num_1"), TF.Add(), TF.Var("num_2"), TF.Return(), TF.Var("result"), TF.EndBody(), TF.Print(), TF.Var("add"), TF.Open(), TF.Num(23), TF.Comma(), TF.Num(55), TF.Close()];

            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
            Statement functionDeclarationStatement = statements[0];
            Statement printStatement = statements[1];
            functionDeclarationStatement.Tokens.Count.Should().Be(19);
            printStatement.Tokens.Count().Should().Be(7);
        }
    }
}

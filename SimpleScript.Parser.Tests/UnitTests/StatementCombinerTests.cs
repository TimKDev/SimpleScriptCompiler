using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests
{
    public class StatementCombinerTests
    {
        private readonly StatementCombiner _sut = StatementCombinerFactory.Create();

        [Fact]
        public void ShouldCreateTwoStatements_GivenAssignmentAndPrint()
        {
            List<Token> tokens =
                [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Str("Hello World"), TF.Print(), TF.Var("hello")];
            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
        }

        [Fact]
        public void ShouldCreateTwoStatementsWithTokens_GivenAssignmentAndPrint()
        {
            List<Token> tokens =
                [TF.Let(), TF.Var("hello"), TF.Assign(), TF.Str("Hello World"), TF.Print(), TF.Var("hello")];
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
            List<Token> tokens =
            [
                TF.Func(), TF.Var("add"), TF.Open(), TF.Int(), TF.Var("num_1"), TF.Comma(), TF.Int(), TF.Var("num_2"),
                TF.Close(), TF.Body(), TF.Let(), TF.Var("result"), TF.Assign(), TF.Var("num_1"), TF.Add(),
                TF.Var("num_2"), TF.Return(), TF.Var("result"), TF.EndBody(), TF.Print(), TF.Var("add"), TF.Open(),
                TF.Num(23), TF.Comma(), TF.Num(55), TF.Close()
            ];

            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
            Statement functionDeclarationStatement = statements[0];
            Statement printStatement = statements[1];
            functionDeclarationStatement.Tokens.Count.Should().Be(19);
            printStatement.Tokens.Count().Should().Be(7);
        }

        [Fact]
        public void ShouldCreateTwoStatementsWithTokens_GivenFunctionInvocation()
        {
            List<Token> tokens =
            [
                TF.Func(), TF.Var("hello"), TF.Open(), TF.Close(), TF.Body(), TF.Print(), TF.Str("Hello World"),
                TF.EndBody(), TF.Var("hello"), TF.Open(), TF.Close()
            ];

            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
        }

        [Fact]
        public void ShouldCreateTwoStatementsWithTokens_GivenInvocationFunctionInvocation()
        {
            List<Token> tokens =
            [
                TF.Var("hello"), TF.Open(), TF.Close(), TF.Func(), TF.Var("hello"), TF.Open(), TF.Close(), TF.Body(),
                TF.Print(), TF.Str("Hello World"), TF.EndBody()
            ];

            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
        }


        [Fact]
        public void ShouldCreateThreeStatementsWithTokens_GivenThreeUsedAdditions()
        {
            List<Token> tokens =
            [
                TF.Let(), TF.Var("hello"), TF.Assign(), TF.Num(10),
                TF.Let(), TF.Var("hello2"), TF.Assign(), TF.Num(10),
                TF.Let(), TF.Var("hello3"), TF.Assign(), TF.Num(10),
            ];

            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(3);
        }


        [Fact]
        public void ShouldCreateTwoStatementsWithTokens_GivenComplexExpressions()
        {
            List<Token> tokens =
            [
                TF.Let(), TF.Var("hello3"), TF.Assign(), TF.Num(11), TF.Let(), TF.Var("hello"), TF.Assign(), TF.Num(10),
                TF.Mul(), TF.Open(), TF.Num(5), TF.Add(), TF.Var("hello3"), TF.Close()
            ];

            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
        }

        [Fact]
        public void ShouldCreateTwoStatementsWithTokens_GivenPrintWithExpression()
        {
            List<Token> tokens =
            [
                TF.Print(), TF.Num(10), TF.Mul(), TF.Open(), TF.Num(5), TF.Add(), TF.Num(6), TF.Close(),
                TF.Let(), TF.Var("hello3"), TF.Assign(), TF.Open(), TF.Num(10), TF.Close()
            ];

            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
        }

        [Fact]
        public void ShouldCreateTwoStatementsWithTokens_GivenExpressionWithFunctionInvocation()
        {
            List<Token> tokens =
            [
                TF.Func(), TF.Var("hello"), TF.Open(), TF.Close(), TF.Body(), TF.Print(), TF.Str("Hello World"),
                TF.EndBody(),
                TF.Let(), TF.Var("test"), TF.Assign(), TF.Var("hello"), TF.Open(), TF.Close(), TF.Add(), TF.Str("Hello World"),
            ];

            List<Statement> statements = ErrorHelper.AssertResultSuccess(_sut.CreateStatements(tokens));
            statements.Count.Should().Be(2);
        }
    }
}
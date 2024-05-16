using FluentAssertions;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScriptCompiler.Tests
{
    public class LexerTests
    {
        private readonly Lexer _sut = new();

        [Fact]
        public void ConvertToTokens_ShouldConvertToTokens_GivenSimpleVariableAssignment()
        {
            var inputString = "LET a = 10";
            var result = _sut.ConvertToTokens(inputString, 1).ToArray();
            result.Should().HaveCount(4);
            result[0].TokenType.Should().Be(TokenType.LET);
            result[1].TokenType.Should().Be(TokenType.Variable);
            result[2].TokenType.Should().Be(TokenType.ASSIGN);
            result[3].TokenType.Should().Be(TokenType.Number);
        }

        [Fact]
        public void ConvertToTokens_ShouldConvertTo47Tokens_GivenFibonacciProgram()
        {
            var inputString = File.ReadAllText("Testprograms/fibonacci.simple");
            var result = _sut.ConvertToTokens(inputString, 1).ToArray();
            result.Should().HaveCount(47);
            result[0].TokenType.Should().Be(TokenType.PRINT);
            result[0].Value.Should().Be(null);
            result[1].TokenType.Should().Be(TokenType.String);
            result[1].Value.Should().Be("How many fibonacci numbers do you want?");
            result[2].TokenType.Should().Be(TokenType.INPUT);
            result[2].Value.Should().Be(null);
            result[3].TokenType.Should().Be(TokenType.Variable);
            result[3].Value.Should().Be("nums");
            result[4].TokenType.Should().Be(TokenType.IF);
            result[4].Value.Should().Be(null);
            result[5].TokenType.Should().Be(TokenType.Variable);
            result[5].Value.Should().Be("nums");
            result[6].TokenType.Should().Be(TokenType.SMALLER);
            result[6].Value.Should().Be(null);
            result[7].TokenType.Should().Be(TokenType.Number);
            result[7].Value.Should().Be("0");
        }

        [Fact]
        public void ConvertToTokens_ShouldConvertToTwoTokens_GivenVariableTokenAtEnd()
        {
            var inputString = "INPUT nums";
            var result = _sut.ConvertToTokens(inputString, 1);
            result.Should().HaveCount(2);
            result[0].TokenType.Should().Be(TokenType.INPUT);
            result[1].TokenType.Should().Be(TokenType.Variable);
        }

        [Fact]
        public void ConvertToTokens_ShouldConvertToFourTokens_GivenIfConstraint()
        {
            var inputString = "IF nums < 0";
            var result = _sut.ConvertToTokens(inputString, 1);
            result.Should().HaveCount(4);
            result[0].TokenType.Should().Be(TokenType.IF);
            result[1].TokenType.Should().Be(TokenType.Variable);
            result[2].TokenType.Should().Be(TokenType.SMALLER);
            result[3].TokenType.Should().Be(TokenType.Number);
        }

        [Fact]
        public void ConvertToTokens_TokensShouldContainLinenumber_GivenIfConstraint()
        {
            var inputString = "IF nums < 0";
            var result = _sut.ConvertToTokens(inputString, 2);
            result[0].Line.Should().Be(2);
            result[1].Line.Should().Be(2);
            result[2].Line.Should().Be(2);
            result[3].Line.Should().Be(2);
        }

        [Fact]
        public void ConvertToTokens_BracketsShouldBeConverted_GivenMathExpression()
        {
            var inputString = "a = (2 + 4)*7";
            var result = _sut.ConvertToTokens(inputString, 5);
            result[0].TokenType.Should().Be(TokenType.Variable);
            result[1].TokenType.Should().Be(TokenType.ASSIGN);
            result[2].TokenType.Should().Be(TokenType.OPEN_BRACKET);
            result[3].TokenType.Should().Be(TokenType.Number);
            result[4].TokenType.Should().Be(TokenType.PLUS);
            result[5].TokenType.Should().Be(TokenType.Number);
            result[6].TokenType.Should().Be(TokenType.CLOSED_BRACKET);
            result[7].TokenType.Should().Be(TokenType.MULTIPLY);
            result[8].TokenType.Should().Be(TokenType.Number);
        }

        [Fact]
        public void ConvertToTokens_ShouldFindVariable_GivenExpressionWithoutWhitespace()
        {
            var inputString = "LET a=2";
            var result = _sut.ConvertToTokens(inputString, 5);
            result[0].TokenType.Should().Be(TokenType.LET);
            result[1].TokenType.Should().Be(TokenType.Variable);
            result[1].Value.Should().Be("a");
            result[2].TokenType.Should().Be(TokenType.ASSIGN);
            result[3].TokenType.Should().Be(TokenType.Number);
            result[3].Value.Should().Be("2");
        }

    }
}
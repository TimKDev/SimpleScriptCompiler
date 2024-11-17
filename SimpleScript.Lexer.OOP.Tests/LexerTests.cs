using FluentAssertions;
using Xunit;

namespace SimpleScript.Lexer.OOP.Tests;

public class LexerTests
{
    private readonly Lexer _sut = new Lexer();

    [Fact]
    public void ShouldReturnStringToken()
    {
        var input = "\"Hello World\"";
        var result = _sut.Tokenize(input);
        result.Tokens.Count.Should().Be(1);
        var stringToken = result.Tokens[0];
        stringToken.TokenType.Should().Be(TokenType.String);
        ((StringToken)stringToken).StringValue.Should().Be("Hello World");
    }
}
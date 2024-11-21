using FluentAssertions;
using Xunit;

namespace SimpleScript.Lexer.OOP.Tests;

public class LexerTests
{
    private readonly Lexer _sut = new Lexer();

    /*[Theory]
    [InlineData("Hello World")]
    [InlineData("Testw233")]
    public void ShouldReturnStringToken(string stringInput)
    {
        var input = $"\"{stringInput}\"";
        var result = _sut.Tokenize(input);
        result.Tokens.Count.Should().Be(1);
        var stringToken = result.Tokens[0];
        stringToken.TokenType.Should().Be(TokenType.String);
        ((StringToken)stringToken).StringValue.Should().Be(stringInput);
    }*/
}
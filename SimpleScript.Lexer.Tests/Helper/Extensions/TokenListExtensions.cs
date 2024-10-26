using FluentAssertions;

namespace SimpleScript.Lexer.Tests.Helper.Extensions;

public static class TokenListExtensions
{
    public static void Assert(this List<Token> tokens, params Token[] expectedTokens)
    {
        tokens.Count.Should().Be(expectedTokens.Length);
        tokens.Should().BeEquivalentTo(expectedTokens, options => options
            .Including(token => token.TokenType)
            .Including(token => token.Value)
            .WithStrictOrdering()
        );
    }
}
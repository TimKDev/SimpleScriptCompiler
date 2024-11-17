using SimpleScript.Lexer.OOP;
using Xunit;

namespace TestProject1;

public class LexerTests
{
    private readonly Lexer _sut = new Lexer();

    [Fact]
    public void ShouldReturnStringToken()
    {
        var input = "\"Hello World\"";
        var result = _sut.Tokenize(input);
         
    }
}
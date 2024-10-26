using SimpleScript.Lexer.Tests.Helper.Extensions;
using SimpleScript.Lexer.Tests.Helper.Factories;
using TF = SimpleScript.Parser.Tests.Helper.Factories.TokenFactory;

namespace SimpleScript.Lexer.Tests
{
    public class LexerTests
    {
        private readonly Lexer _lexer = LexerFactory.Create();

        [Fact]
        public void InStringsAlsoKeywordsAndWhitespaceIsAllowed()
        {
            var stringToTokenize = "LET name = \"  +-\t Hallo FUNC\"";
            var result = _lexer.ConvertToTokens(stringToTokenize, 0);

            result.Assert(TF.Let(), TF.Var("name"), TF.Assign(), TF.Str("  +-\t Hallo FUNC"));
        }

        [Fact]
        public void FunctionHeader()
        {
            var stringToTokenize = "FUNC addString(string string_1, string string_2)";
            var result = _lexer.ConvertToTokens(stringToTokenize, 0);

            result.Assert(TF.Func(), TF.Var("addString"), TF.Open(), TF.String(), TF.Var("string_1"), TF.Comma(),
                TF.String(), TF.Var("string_2"), TF.Close());
        }
    }
}
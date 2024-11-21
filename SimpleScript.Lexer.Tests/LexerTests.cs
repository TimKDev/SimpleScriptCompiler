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

            result.Assert(TF.Func(), TF.Var("addString"), TF.Open(), TF.StringArg(), TF.Var("string_1"), TF.Comma(),
                TF.StringArg(), TF.Var("string_2"), TF.Close());
        }

        [Fact]
        public void VariableAssignmentToBoolean()
        {
            var stringToTokenize = "LET isDev = TRUE";
            var result = _lexer.ConvertToTokens(stringToTokenize, 0);

            result.Assert(TF.Let(), TF.Var("isDev"), TF.Assign(), TF.True());
        }

        [Fact]
        public void ConvertIfCondition()
        {
            var stringToTokenize = "IF name == \"Tim\" DO PRINT \"Hallo Tim\" ENDIF";
            var result = _lexer.ConvertToTokens(stringToTokenize, 0);

            result.Assert(TF.If(), TF.Var("name"), TF.Equal(), TF.Str("Tim"), TF.Do(), TF.Print(), TF.Str("Hallo Tim"),
                TF.EndIf());
        }

        [Fact]
        public void ConvertWhileLoop()
        {
            var stringToTokenize = "WHILE num <= 5 REPEAT PRINT num num = num + 1 ENDWHILE";
            var result = _lexer.ConvertToTokens(stringToTokenize, 0);

            result.Assert(TF.While(), TF.Var("num"), TF.SmallerOrEqual(), TF.Num(5), TF.Repeat(), TF.Print(),
                TF.Var("num"), TF.Var("num"), TF.Assign(), TF.Var("num"), TF.Add(), TF.Num(1), TF.EndWhile());
        }
    }
}
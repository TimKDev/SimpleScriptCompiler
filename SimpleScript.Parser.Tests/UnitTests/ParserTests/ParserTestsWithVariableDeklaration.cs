using SimpleScript.Lexer;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParserTestsWithVariableDeklaration
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParseTokens_ShouldGenerateVariableDeklarationNode_GivenProgramTokens()
        {
            List<Token> programTokens = [TF.Let(), TF.Var("name")];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage("Invalid usage of Let keyword. Let should be followed by a assign to define an initial value.", 1)]);
        }

        [Fact]
        public void ParseTokens_ShouldGenerateError_GivenLetWithoutAnyThing()
        {
            List<Token> programTokens = [TF.Let()];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage("Invalid usage of Let keyword. Let should be followed by a variable name and an initial value.", 1)]);
        }

        [Fact]
        public void ParseTokens_ShouldGenerateError_GivenLetWithoutVariable()
        {
            List<Token> programTokens = [TF.Let(), TF.Num(33)];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage("Invalid usage of Let keyword. Let should be followed by a variable name and an initial value.", 1)]);
        }

        [Fact]
        public void ParseTokens_ShouldGenerateError_GivenLetWithVariableNameEqualsNull()
        {
            List<Token> programTokens = [TF.Let(), TF.Var(null)];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage("Invalid usage of Let keyword. Let should be followed by a variable name not equals to null.", 1)]);
        }


    }
}

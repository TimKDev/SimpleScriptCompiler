using FluentAssertions;
using SimpleScript.Compiler.Tests.Helper;
using SimpleScript.Compiler.Tests.Helper.Factories;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    public class FunctionDefinition
    {
        private string _programPath = "ExamplePrograms/FunctionDefinition.simple";
        private Command.ExecuteCommand _sut = ExecuteCommandFactory.Create();

        [Fact]
        public void ShouldCompileSuccessfully()
        {
            EntertainingErrors.Result result = _sut.Execute([_programPath]);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void ShouldCreateCorrectCCode()
        {
            string expectedCCode = CompilerTestHelper.ConvertToCCode([
                "int add(int num_1, int num_2)",
                "{",
                "int result = num_1 + num_2;",
                "return result;",
                "}",
                "printf(\"Hello SimpleScript!\");"
            ]);
            _sut.Execute([_programPath]);
            string resultingCCode = File.ReadAllText("FunctionDefinition.c");
            CompilerTestHelper.AssertNormalizedStrings(resultingCCode, expectedCCode);
        }
    }
}
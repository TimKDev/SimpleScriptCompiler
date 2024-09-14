using FluentAssertions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    public class SimpleVariableDeklarationAndPrint
    {
        private string _programPath = "ExamplePrograms/SimpleVariableDeklarationAndPrint.simple";
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
                "char temp_1[4];",
                "strcpy(temp_1, \"T\");",
                "strcat(temp_1, \"I\");",
                "strcat(temp_1, \"M\");",
                "char *name = temp_1;",
                "char temp_2[10];",
                "strcpy(temp_2, \"Hello \");",
                "strcat(temp_2, name);",
                "printf(temp_2);",
            ]);
            _sut.Execute([_programPath]);
            string resultingCCode = File.ReadAllText("SimpleVariableDeklarationAndPrint.c");
            CompilerTestHelper.AssertNormalizedStrings(resultingCCode, expectedCCode);
        }
    }
}

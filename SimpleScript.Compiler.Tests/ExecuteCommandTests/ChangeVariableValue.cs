using FluentAssertions;
using SimpleScript.Compiler.Command;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    public class ChangeVariableValue
    {
        private string _programPath = "ExamplePrograms/ChangeVariableValue.simple";
        private ExecuteCommand _sut = ExecuteCommandFactory.Create();

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
                "char *firstName = \"Tim\";",
                "char temp_1[13];",
                "strcpy(temp_1, firstName);",
                "strcat(temp_1, \" Kempkens\");",
                "printf(temp_1);",
                "firstName = \"Caro\";",
                "char temp_2[14];",
                "strcpy(temp_2, firstName);",
                "strcat(temp_2, \" Kempkens\");",
                "printf(temp_2);"

            ]);
            _sut.Execute([_programPath]);
            string resultingCCode = File.ReadAllText("ChangeVariableValue.c");
            CompilerTestHelper.AssertNormalizedStrings(resultingCCode, expectedCCode);
        }
    }
}



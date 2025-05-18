using FluentAssertions;
using SimpleScript.Compiler.Command;
using SimpleScript.Compiler.Tests.Helper.Extensions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    [Collection("Sequential")]
    public class ChangeVariableValue : IDisposable
    {
        private const string ProgramName = "ChangeVariableValue";
        private ExecuteCommand _sut = ExecuteCommandFactory.Create();

        public ChangeVariableValue()
        {
            CleanUp();
        }

        [Fact]
        public void ShouldCompileSuccessfully()
        {
            EntertainingErrors.Result result = _sut.Execute([ProgramName.ToExampleProgramPath()]);
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
                "printf(\"%s\", temp_1);",
                "fflush(stdout);",
                "firstName = \"Caro\";",
                "char temp_2[14];",
                "strcpy(temp_2, firstName);",
                "strcat(temp_2, \" Kempkens\");",
                "printf(\"%s\",temp_2);",
                "fflush(stdout);"
            ]);
            _sut.Execute([ProgramName.ToExampleProgramPath()]);
            string resultingCCode = File.ReadAllText(ProgramName.AddCExtension());
            CompilerTestHelper.AssertNormalizedStrings(resultingCCode, expectedCCode);
        }

        public void Dispose()
        {
            CleanUp();
        }


        private void CleanUp()
        {
            CompilerTestCleanup.DeleteFiles(ProgramName);
        }
    }
}
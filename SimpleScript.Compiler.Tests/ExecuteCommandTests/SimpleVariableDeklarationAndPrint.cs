using FluentAssertions;
using SimpleScript.Compiler.Tests.Helper.Extensions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    [Collection("Sequential")]
    public class SimpleVariableDeklarationAndPrint : IDisposable
    {
        private const string ProgramName = "SimpleVariableDeklarationAndPrint";
        private readonly Command.ExecuteCommand _sut = ExecuteCommandFactory.Create();

        public SimpleVariableDeklarationAndPrint()
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
                "char temp_1[4];",
                "strcpy(temp_1, \"T\");",
                "strcat(temp_1, \"I\");",
                "strcat(temp_1, \"M\");",
                "char *name = temp_1;",
                "char temp_2[10];",
                "strcpy(temp_2, \"Hello \");",
                "strcat(temp_2, name);",
                "printf(\"%s\",temp_2);",
                "fflush(stdout);",
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
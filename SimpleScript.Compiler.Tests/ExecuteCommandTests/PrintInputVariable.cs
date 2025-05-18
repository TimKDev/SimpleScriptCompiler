using FluentAssertions;
using SimpleScript.Compiler.Command;
using SimpleScript.Compiler.Tests.Helper.Extensions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    [Collection("Sequential")]
    public class PrintInputVariable : IDisposable
    {
        private const string ProgramName = "PrintInputVariable";
        private ExecuteCommand _sut = ExecuteCommandFactory.Create();

        public PrintInputVariable()
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
                "printf(\"%s\",\"Enter your name:\");",
                "fflush(stdout);",
                "char temp_1[200];",
                "fgets(temp_1, sizeof(temp_1), stdin);",
                "size_t temp_2 = strlen(temp_1);",
                "if (temp_2 > 0 && temp_1[temp_2 - 1] == '\\n')",
                "{",
                " temp_1[temp_2 - 1] = '\\0';",
                "}",
                "char *name = temp_1;",
                "char temp_3[231];",
                "strcpy(temp_3, \"Hello \");",
                "strcat(temp_3, name);",
                "strcat(temp_3, \"! My name is SimpleScript\");",
                "printf(\"%s\",temp_3);",
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
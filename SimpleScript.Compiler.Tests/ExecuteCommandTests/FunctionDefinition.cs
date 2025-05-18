using FluentAssertions;
using SimpleScript.Compiler.Tests.Helper.Extensions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    [Collection("Sequential")]
    public class FunctionDefinition : IDisposable
    {
        private const string ProgramName = "FunctionDefinition";
        private Command.ExecuteCommand _sut = ExecuteCommandFactory.Create();

        public FunctionDefinition()
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
                "printf(\"%s\", \"Result of 23 + 55 is \");",
                "fflush(stdout);",
                "printf(\"%d\", add(23, 55));",
                "fflush(stdout);"
            ], [
                "int add(int num_1, int num_2)",
                "{",
                "int result = (num_1 + num_2);",
                "return result;",
                "}",
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
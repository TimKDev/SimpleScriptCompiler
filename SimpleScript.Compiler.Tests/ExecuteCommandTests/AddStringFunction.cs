using SimpleScript.Compiler.Tests.Helper.Extensions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    [Collection("Sequential")]
    public class FunctionWithoutArguments : IDisposable
    {
        private const string ProgramName = "FunctionWithoutArguments";
        private readonly Command.ExecuteCommand _sut = ExecuteCommandFactory.Create();

        public FunctionWithoutArguments()
        {
            CleanUp();
        }

        [Fact]
        public void ShouldCompileSuccessfully()
        {
            EntertainingErrors.Result result = _sut.Execute([ProgramName.ToExampleProgramPath()]);
            result.AssertSuccess();
        }

        [Fact]
        public void ShouldCreateCorrectCCode()
        {
            string expectedCCode = CompilerTestHelper.ConvertToCCode(
            [
                "sayHello();",
            ], [
                "void sayHello()",
                "{",
                "printf(\"%s\", \"Hallo\");",
                "fflush(stdout);",
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

    [Collection("Sequential")]
    public class AddStringFunction : IDisposable
    {
        private const string ProgramName = "AddStringFunction";
        private readonly Command.ExecuteCommand _sut = ExecuteCommandFactory.Create();

        public AddStringFunction()
        {
            CleanUp();
        }

        [Fact]
        public void ShouldCompileSuccessfully()
        {
            EntertainingErrors.Result result = _sut.Execute([ProgramName.ToExampleProgramPath()]);
            result.AssertSuccess();
        }

        [Fact]
        public void ShouldCreateCorrectCCode()
        {
            string expectedCCode = CompilerTestHelper.ConvertToCCode(
            [
                "printf(\"%s\", addStrings(\"Hallo \",  \"String Addition\"));",
                "fflush(stdout);"
            ], [
                "char * addStrings(char string_1[], char string_2[])",
                "{",
                "char *temp_1 = (char *)malloc((strlen(string_1) + strlen(string_2) + 1) * sizeof(char));",
                "add_to_list(temp_1);",
                "strcpy(temp_1, string_1);",
                "strcat(temp_1, string_2);",
                "char *result = temp_1;",
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
using SimpleScript.Compiler.Tests.Helper.Extensions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    public class AddStringFunction
    {
        private readonly string _programPath = "ExamplePrograms/AddStringFunction.simple";
        private readonly Command.ExecuteCommand _sut = ExecuteCommandFactory.Create();

        [Fact]
        public void ShouldCompileSuccessfully()
        {
            EntertainingErrors.Result result = _sut.Execute([_programPath]);
            result.AssertSuccess();
        }

        [Fact]
        public void ShouldCreateCorrectCCode()
        {
            string expectedCCode = CompilerTestHelper.ConvertToCCode(
            [
                "printf(addStrings(\"Hallo \",  \"String Addition\"));",
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
            _sut.Execute([_programPath]);
            string resultingCCode = File.ReadAllText("AddStringFunction.c");
            CompilerTestHelper.AssertNormalizedStrings(resultingCCode, expectedCCode);
        }
    }
}
using FluentAssertions;
using SimpleScript.Compiler.Command;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    public class PrintInputVariable
    {
        private string _programPath = "ExamplePrograms/PrintInputVariable.simple";
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
                "printf(\"Enter your name:\");",
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
                "printf(temp_3);"
            ]);
            _sut.Execute([_programPath]);
            string resultingCCode = File.ReadAllText("PrintInputVariable.c");
            CompilerTestHelper.AssertNormalizedStrings(resultingCCode, expectedCCode);
        }
    }
}



using FluentAssertions;
using SimpleScript.Compiler.Tests.Helper.Extensions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    [Collection("Sequential")]
    public class Fibonacci : IDisposable
    {
        private const string ProgramName = "Fibonacci";
        private Command.ExecuteCommand _sut = ExecuteCommandFactory.Create();

        public Fibonacci()
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
                //PRINT "How many fibonacci numbers do you want?"
                "printf(\"%s\",\"How many fibonacci numbers do you want?\");",
                "fflush(stdout);",
                //INPUT numsInput
                "char temp_1[200];",
                "fgets(temp_1, sizeof(temp_1), stdin);",
                "size_t temp_2 = strlen(temp_1);",
                "if (temp_2 > 0 && temp_1[temp_2 - 1] == '\\n')",
                "{",
                "temp_1[temp_2 - 1] = '\\0';",
                "}",
                "char *numsInput = temp_1;",
                //LET nums = ToNumber(numsInput)
                "int nums = ToNumber(numsInput);",
                //IF nums < 0
                //    PRINT "Number should be greater or equal to zero!"
                //ENDIF
                "if((nums < 0))",
                "{",
                "printf(\"%s\",\"Number should be greater or equal to zero!\");",
                "fflush(stdout);",
                "}",
                //LET a = 0
                "int a = 0;",
                //LET b = 1
                "int b = 1;",
                //WHILE nums > 0 REPEAT
                //    PRINT a
                //    LET c = a + b
                //    LET a = b
                //    LET b = c
                //    LET nums = nums - 1
                //ENDWHILE
                "while((nums > 0))",
                "{",
                "printf(\"%d\", a);",
                "fflush(stdout);",
                "int c = (a + b);",
                "a = b;",
                "b = c;",
                "nums = (nums - 1);",
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
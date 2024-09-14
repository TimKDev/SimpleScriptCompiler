using FluentAssertions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    public class Fibonacci
    {
        private string _programPath = "ExamplePrograms/Fibonacci.simple";
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
                //PRINT "How many fibonacci numbers do you want?"
                "printf(\"How many fibonacci numbers do you want?\")",
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
                "int nums = atoi(numsInput);",
                //IF nums < 0
                //    PRINT "Number should be greater or equal to zero!"
                //ENDIF
                "if(ToNumber(nums) < 0)",
                "{",
                "printf(\"Number should be greater or equal to zero!\")",
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
                "while(nums > 0)",
                "{",
                "printf(a);",
                "int c = a + b;",
                "int a = b;",
                "int b = c;",
                "int nums = nums - 1;",
                "}",


                "printf(\"Hello SimpleScript!\");"
            ]);
            _sut.Execute([_programPath]);
            string resultingCCode = File.ReadAllText("Fibonacci.c");
            CompilerTestHelper.AssertNormalizedStrings(resultingCCode, expectedCCode);
        }
    }
}

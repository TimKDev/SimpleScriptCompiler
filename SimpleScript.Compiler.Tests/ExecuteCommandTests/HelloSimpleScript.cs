using FluentAssertions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests
{
    public class HelloSimpleScript
    {
        private string _programPath = "ExamplePrograms/HelloSimpleScript.simple";
        private readonly Command.ExecuteCommand _sut = ExecuteCommandFactory.Create();

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
                "printf(\"Hello SimpleScript!\");"
            ]);
            _sut.Execute([_programPath]);
            string resultingCCode = File.ReadAllText("HelloSimpleScript.c");
            CompilerTestHelper.AssertNormalizedStrings(resultingCCode, expectedCCode);
        }
    }
}

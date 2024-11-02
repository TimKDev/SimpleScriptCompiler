using FluentAssertions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests;

public class IfConditionWithPrint
{
    private readonly string _programPath = "ExamplePrograms/IfConditionWithPrint.simple";
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
            "char *name = \"Tim\";",
            "if((name == \"Tim\"))",
            "{",
            "printf(\"Hello Tim\");",
            "}",
            "if((name == \"Carolin\"))",
            "{",
            "printf(\"Hello Carolin\");",
            "}",
        ]);
        _sut.Execute([_programPath]);
        string resultingCCode = File.ReadAllText("IfConditionWithPrint.c");
        CompilerTestHelper.AssertNormalizedStrings(resultingCCode, expectedCCode);
    }
}
using FluentAssertions;
using SimpleScript.Compiler.Tests.Helper.Extensions;
using SimpleScript.Compiler.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Compiler.Tests.ExecuteCommandTests;

[Collection("Sequential")]
public class IfConditionWithPrint : IDisposable
{
    private const string ProgramName = "IfConditionWithPrint";
    private readonly Command.ExecuteCommand _sut = ExecuteCommandFactory.Create();

    public IfConditionWithPrint()
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
            "char *name = \"Tim\";",
            "if((name == \"Tim\"))",
            "{",
            "printf(\"%s\",\"Hello Tim\");",
            "fflush(stdout);",
            "}",
            "if((name == \"Carolin\"))",
            "{",
            "printf(\"%s\", \"Hello Carolin\");",
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
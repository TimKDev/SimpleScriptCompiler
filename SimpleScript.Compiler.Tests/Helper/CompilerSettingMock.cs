using Microsoft.Extensions.Options;

namespace SimpleScript.Compiler.Tests.Helper.Factories;

internal class CompilerSettingMock : IOptions<CompilerSettings>
{
    public CompilerSettings Value { get; }

    public CompilerSettingMock(bool createOutputFiles)
    {
        Value = new CompilerSettings()
        {
            CreateOutputFiles = createOutputFiles,
        };
    }
}
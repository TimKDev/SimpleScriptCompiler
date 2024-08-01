using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Extensions
{
    internal static class ConverterToCCodeExtensions
    {
        public static void AssertConverterToCCode(this ProgramConverterToC converter, ProgramNode programNode, string[] expectedBody)
        {
            string expectedResult = @$"
                #include <stdio.h>
                #include <string.h>
                int main() {{
                    {string.Join("\n", expectedBody)}
                    return 0;
                }}
            ";

            EntertainingErrors.Result<string> result = converter.ConvertToCCode(programNode);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(expectedResult);
        }
    }
}

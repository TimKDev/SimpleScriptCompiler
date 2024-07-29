using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Extensions
{
    internal static class ConverterToCCodeExtensions
    {
        public static void AssertConverterToCCode(this ConverterToCCode converter, ProgramNode programNode, string[] expectedBody)
        {
            string expectedResult = @$"
                #include <stdio.h>
                int main() {{
                    {string.Join("\n", expectedBody)}
                    return 0;
                }}
            ";

            string result = converter.ConvertToCCode(programNode);
            result.Should().Be(expectedResult);
        }
    }
}

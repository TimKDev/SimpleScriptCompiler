using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Extensions
{
    internal static class ConverterToCCodeExtensions
    {
        public static void AssertConverterToCCode(this ProgramConverterToC converter, ProgramNode programNode, string[] expectedBody, string[] functionDeclarations)
        {
            string expectedResult = functionDeclarations.Length != 0 ? @$"
                #include <stdio.h>
                #include <string.h>
                {string.Join("\n", functionDeclarations)}
                int main() {{
                    {string.Join("\n", expectedBody)}
                    return 0;
                }}
            " : @$"
                #include <stdio.h>
                #include <string.h>
                int main() {{
                    {string.Join("\n", expectedBody)} 
                    return 0;
                }}
            ";

            EntertainingErrors.Result<string> result = converter.ConvertToCCode(programNode);
            _ = result.IsSuccess.Should().BeTrue();
            result.Value.AssertWithoutWhitespace(expectedResult);
        }

        public static void AssertConverterToCCode(this ProgramConverterToC converter, ProgramNode programNode, string[] expectedMainBody)
        {
            AssertConverterToCCode(converter, programNode, expectedMainBody, []);
        }
    }
}

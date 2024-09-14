using FluentAssertions;
using SimpleScript.Parser.Nodes;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Adapter.C.Tests.Helper.Extensions
{
    internal static class ConverterToCCodeExtensions
    {
        public static void AssertConverterToCCode(this ProgramConverterToC converter, ProgramNode programNode, string expectedBody, string? functionDeclarations = null)
        {
            var expectedResult = CompilerTestHelper.ConvertToCCode(expectedBody, functionDeclarations);
            EntertainingErrors.Result<string> result = converter.ConvertToCCode(programNode);
            _ = result.IsSuccess.Should().BeTrue();
            result.Value.AssertWithoutWhitespace(expectedResult);
        }

        public static void AssertConverterToCCode(this ProgramConverterToC converter, ProgramNode programNode, string[] expectedBody, string[]? functionDeclarations = null)
        {
            var expectedResult = CompilerTestHelper.ConvertToCCode(expectedBody, functionDeclarations ?? []);
            EntertainingErrors.Result<string> result = converter.ConvertToCCode(programNode);
            _ = result.IsSuccess.Should().BeTrue();
            result.Value.AssertWithoutWhitespace(expectedResult);
        }
    }
}

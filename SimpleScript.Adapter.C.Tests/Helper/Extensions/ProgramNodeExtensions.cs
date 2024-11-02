using FluentAssertions;
using SimpleScript.Parser.Nodes;
using SimpleScript.Tests.Shared;
using Xunit.Abstractions;

namespace SimpleScript.Adapter.C.Tests.Helper.Extensions
{
    internal static class ConverterToCCodeExtensions
    {
        public static void AssertConverterToCCode(this ProgramConverterToC converter, ProgramNode programNode,
            string[] expectedBody, string[]? functionDeclarations = null, ITestOutputHelper? testOutputHelper = null)
        {
            var expectedResult = CompilerTestHelper.ConvertToCCode(expectedBody, functionDeclarations ?? []);
            EntertainingErrors.Result<string> result = converter.ConvertToCCode(programNode);
            if (!result.IsSuccess)
            {
                testOutputHelper?.WriteLine(string.Join(". ", result.Errors));
            }

            result.IsSuccess.Should().BeTrue();
            result.Value.AssertWithoutWhitespace(expectedResult);
        }

        public static void AssertErrorsConverterToCCode(this ProgramConverterToC converter, ProgramNode programNode,
            string[] errors, ITestOutputHelper testOutputHelper)
        {
            EntertainingErrors.Result<string> result = converter.ConvertToCCode(programNode);
            testOutputHelper?.WriteLine(string.Join(". ", result.Errors));
            result.IsSuccess.Should().BeFalse();
            result.Errors.Select(e => e.Message).Should().BeEquivalentTo(errors);
        }
    }
}
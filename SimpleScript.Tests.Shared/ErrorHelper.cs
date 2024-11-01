using EntertainingErrors;
using FluentAssertions;
using Xunit.Abstractions;

namespace SimpleScript.Tests.Shared
{
    public static class ErrorHelper
    {
        public static TValue AssertResultSuccess<TValue>(Result<TValue> result,
            ITestOutputHelper? testOutputHelper = null)
        {
            if (!result.IsSuccess && testOutputHelper != null)
            {
                testOutputHelper.WriteLine(string.Join(", ", result.Errors.Select(e => e.Message)));
            }

            result.IsSuccess.Should().BeTrue();
            return result.Value;
        }

        public static void AssertErrors<TValue>(Result<TValue> result, string[] expectedErrorMessages)
        {
            result.IsSuccess.Should().BeFalse();
            result.Errors.Select(error => error.Message).Should()
                .BeEquivalentTo(expectedErrorMessages, options => options.WithoutStrictOrdering());
        }

        public static string CreateErrorMessage(string message, int lineNumber)
        {
            return $"Error Line {lineNumber}: {message}";
        }

        public static string CreateErrorMessage(string message, int lineNumberStart, int lineNumberEnd)
        {
            return $"Error Line {lineNumberStart} - {lineNumberEnd}: {message}";
        }
    }
}
using EntertainingErrors;
using FluentAssertions;

namespace SimpleScript.Parser.Tests.Helper
{
    internal static class ErrorHelper
    {
        public static TValue AssertResultSuccess<TValue>(Result<TValue> result)
        {
            result.IsSuccess.Should().BeTrue();
            return result.Value;
        }

        public static void AssertErrors<TValue>(Result<TValue> result, string[] expectedErrorMessages)
        {
            result.IsSuccess.Should().BeFalse();
            result.Errors.Select(error => error.Message).Should().BeEquivalentTo(expectedErrorMessages, options => options.WithoutStrictOrdering());
        }
    }
}

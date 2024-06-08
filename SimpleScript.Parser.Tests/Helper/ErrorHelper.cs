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
    }
}

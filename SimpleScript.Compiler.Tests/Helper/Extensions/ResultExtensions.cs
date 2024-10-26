using EntertainingErrors;
using FluentAssertions;

namespace SimpleScript.Compiler.Tests.Helper.Extensions;

public static class ResultExtensions
{
    public static void AssertSuccess(this Result result)
    {
        //In this way the error message is shown in the test console in case of failure.
        string.Join("\n", result.Errors).Should().Be(string.Empty);
        result.IsSuccess.Should().BeTrue();
    }
}
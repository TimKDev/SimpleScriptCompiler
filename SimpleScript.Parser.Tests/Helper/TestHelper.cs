using FluentAssertions;

namespace SimpleScript.Parser.Tests.Helper
{
    internal static class TestHelper
    {
        public static T ConvertTo<T>(object input)
        {
            input.Should().BeOfType<T>();
            return (T)input;
        }
    }
}

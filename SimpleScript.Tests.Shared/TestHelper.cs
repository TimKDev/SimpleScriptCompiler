using FluentAssertions;

namespace SimpleScript.Tests.Shared
{
    public static class TestHelper
    {
        public static T ConvertTo<T>(object input)
        {
            input.Should().BeOfType<T>();
            return (T)input;
        }
    }
}

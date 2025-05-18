using FluentAssertions;

namespace SimpleScript.Adapter.C.Tests.Helper.Extensions
{
    public static class StringExtensions
    {
        public static void AssertWithoutWhitespace(this string actual, string expected)
        {
            RemoveWhiteSpace(actual).Should().BeEquivalentTo(RemoveWhiteSpace(expected));
        }

        private static string RemoveWhiteSpace(string myString)
        {
            return new string(myString.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
    }
}
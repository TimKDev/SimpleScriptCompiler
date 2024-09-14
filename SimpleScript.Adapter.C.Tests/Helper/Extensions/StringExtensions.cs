using System.Text.RegularExpressions;
using FluentAssertions;

namespace SimpleScript.Adapter.C.Tests.Helper.Extensions
{
    public static partial class StringExtensions
    {
        public static void AssertWithoutWhitespace(this string actual, string expected)
        {
            RemoveWhiteSpace(actual).Should().BeEquivalentTo(RemoveWhiteSpace(expected));
        }

        private static string RemoveWhiteSpace(string myString)
        {
            return MyRegex().Replace(myString, "");
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex MyRegex();
    }
}

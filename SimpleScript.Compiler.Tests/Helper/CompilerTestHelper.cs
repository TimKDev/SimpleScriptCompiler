using FluentAssertions;
using System.Text.RegularExpressions;

namespace SimpleScript.Compiler.Tests.Helper
{
    internal static class CompilerTestHelper
    {
        public static string ConvertToCCode(string[] expectedBody)
        {
            return @$"
                #include <stdio.h>
                #include <string.h>
                int main() {{
                    {string.Join("\n", expectedBody)}
                    return 0;
                }}
            ";
        }

        public static string NormalizeWhiteSpace(string input)
        {
            return Regex.Replace(input, @"\s+", " ").Trim();
        }

        public static void AssertNormalizedStrings(string actual, string expected)
        {
            CompilerTestHelper.NormalizeWhiteSpace(actual).Should().Be(CompilerTestHelper.NormalizeWhiteSpace(expected));
        }
    }
}

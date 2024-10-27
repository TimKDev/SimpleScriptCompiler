using FluentAssertions;
using System.Text.RegularExpressions;

namespace SimpleScript.Tests.Shared
{
    public static class CompilerTestHelper
    {
        public static string ConvertToCCode(string expectedBody, string? functionDeclaration = null)
        {
            return @$"
                #include <stdio.h>
                #include <string.h>
                #include <stdlib.h>
                #include ""CCode/compiler-helper.h""
                {functionDeclaration}
                int main() {{
                    {expectedBody}
                    free_list();
                    return 0;
                }}
            ";
        }

        public static string ConvertToCCode(string[] expectedBody, string[]? functionDeclarations = null)
        {
            return ConvertToCCode(string.Join("\n", expectedBody),
                functionDeclarations is null ? null : string.Join("\n", functionDeclarations));
        }

        public static string NormalizeWhiteSpace(string input)
        {
            return Regex.Replace(input, @"\s+", " ").Trim();
        }

        public static void AssertNormalizedStrings(string actual, string expected)
        {
            NormalizeWhiteSpace(actual).Should().Be(NormalizeWhiteSpace(expected));
        }
    }
}
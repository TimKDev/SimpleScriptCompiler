﻿namespace SimpleScript.Compiler.Tests.Helper
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
    }
}

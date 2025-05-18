namespace SimpleScript.Compiler.Tests.Helper.Extensions;

internal static class StringExtensions
{
    public static string AddCExtension(this string fileName)
    {
        return $"{fileName}.c";
    }

    public static string ToExampleProgramPath(this string fileName)
    {
        return $"ExamplePrograms/{fileName}.simple";
    }
}
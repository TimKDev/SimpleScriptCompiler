using SimpleScript.Compiler.Tests.Helper.Extensions;

namespace SimpleScript.Compiler.Tests;

public static class CompilerTestCleanup
{
    public static void DeleteFiles(string fileName)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        var cFileName = fileName.AddCExtension();
        if (File.Exists(cFileName))
        {
            File.Delete(cFileName);
        }
    }
}
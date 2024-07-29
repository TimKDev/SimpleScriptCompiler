using SimpleScript.Adapter.Abstractions;
using System.Diagnostics;

namespace SimpleScript.Adapter.C
{
    public class CompileCCode : ICompiler
    {
        public bool Compile(string fileName, string code)
        {
            string cFileName = $"{fileName}.c";
            File.WriteAllText(cFileName, code);

            return CompileCCodeFile(cFileName, fileName);
        }

        private static bool CompileCCodeFile(string cFileName, string executableName)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = "gcc",
                Arguments = $"{cFileName} -o {executableName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new()
            { StartInfo = processStartInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Compiler Output:\n" + output);
                    Console.WriteLine("Compiler Errors:\n" + errors);
                    return false;
                }
            }
        }
    }
}

using SimpleScript.Converter.Abstractions;
using System.Diagnostics;

namespace SimpleScript.Adapter.C
{
    public class CompileCCode : ICompiler
    {
        public void CompileAndExecute(string code)
        {
            string cFileName = "temp.c";
            string executableName = "temp";

            // Write C code to file
            File.WriteAllText(cFileName, code);

            // Compile C code
            bool compilationSuccess = CompileCCodeFile(cFileName, executableName);

            if (compilationSuccess)
            {
                Console.WriteLine("Compilation succeeded.");
                // Optionally, run the compiled executable
                RunExecutable(executableName);
            }
            else
            {
                Console.WriteLine("Compilation failed.");
            }

            // Clean up temporary files
            File.Delete(cFileName);
            if (File.Exists(executableName))
            {
                File.Delete(executableName);
            }
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

        private static void RunExecutable(string executableName)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = executableName,
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

                Console.WriteLine("Executable Output:\n" + output);
                if (!string.IsNullOrEmpty(errors))
                {
                    Console.WriteLine("Executable Errors:\n" + errors);
                }
            }
        }
    }
}

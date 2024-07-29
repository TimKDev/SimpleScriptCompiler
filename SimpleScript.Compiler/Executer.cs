using System.Diagnostics;

namespace SimpleScript.Compiler
{
    /// <summary>
    /// Class for running the .exe file after the compilation.
    /// </summary>
    internal class Executer
    {
        public static void RunExecutable(string executableName)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = executableName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new() { StartInfo = processStartInfo })
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

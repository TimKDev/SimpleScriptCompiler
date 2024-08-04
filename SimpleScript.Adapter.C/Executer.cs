using SimpleScript.Adapter.Abstractions;
using System.Diagnostics;

namespace SimpleScript.Adapter.C
{
    /// <summary>
    /// Class for running the .exe file after the compilation.
    /// </summary>
    public class Executer : IExecuter
    {
        public void RunExecutable(string executableName)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = executableName,
                UseShellExecute = true,
                CreateNoWindow = false
            };

            using Process process = new() { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit();
        }
    }
}

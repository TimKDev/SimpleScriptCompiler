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

            ExecuteProcessWithIoRedirect("gcc", $"-g -c {cFileName} -o build/{fileName}.o");
            ExecuteProcessWithIoRedirect("gcc", "-g -c CCode/compiler-helper.c -o build/compiler-helper.o");

            return ExecuteProcessWithIoRedirect("gcc", $"-o {fileName} build/{fileName}.o build/compiler-helper.o");
        }

        private static bool ExecuteProcessWithIoRedirect(string command, string arguments)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = new();
            process.StartInfo = processStartInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                return true;
            }

            Console.WriteLine("Compiler Output:\n" + output);
            Console.WriteLine("Compiler Errors:\n" + errors);
            return false;
        }
    }
}
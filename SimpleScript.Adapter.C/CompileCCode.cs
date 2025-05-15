using Microsoft.Extensions.Logging;
using SimpleScript.Adapter.Abstractions;
using System.Diagnostics;

namespace SimpleScript.Adapter.C
{
    public class CompileCCode : ICompiler
    {
        private readonly ILogger<CompileCCode> _logger;

        public CompileCCode(ILogger<CompileCCode> logger)
        {
            _logger = logger;
        }

        public bool Compile(string fileName, string code)
        {
            var cFileName = GetCFileName(fileName);
            File.WriteAllText(cFileName, code);

            if (!Directory.Exists("build"))
            {
                Directory.CreateDirectory("build");
            }

            var outputFileName = GetOutputFileName(fileName);

            if (!ExecuteProcessWithIoRedirect("gcc", $"-g -c {cFileName} -o {outputFileName}"))
            {
                _logger.LogError("Failed to compile {CFileName} to object file", cFileName);
                return false;
            }

            if (!ExecuteProcessWithIoRedirect("gcc",
                    "-g -c CCode/compiler-helper.c -o build/compiler-helper.o"))
            {
                _logger.LogError("Failed to compile compiler-helper.c to object file");
                return false;
            }

            bool result =
                ExecuteProcessWithIoRedirect("gcc",
                    $"-o {fileName} build/{fileName}.o build/compiler-helper.o");
            if (!result)
            {
                _logger.LogError("Failed to link object files into executable: {FileName}", fileName);
            }
            else
            {
                _logger.LogInformation("Successfully compiled {FileName}", fileName);
            }

            return result;
        }

        public void Cleanup(string fileName)
        {
            RemoveIfExists(GetCFileName(fileName));
            RemoveIfExists(GetOutputFileName(fileName));
            RemoveIfExists(fileName);
        }

        private static string GetOutputFileName(string fileName)
        {
            var outputFileName = $"build/{fileName}.o";
            return outputFileName;
        }

        private static string GetCFileName(string fileName)
        {
            string cFileName = $"{fileName}.c";
            return cFileName;
        }


        private void RemoveIfExists(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        private bool ExecuteProcessWithIoRedirect(string command, string arguments)
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

            if (!string.IsNullOrEmpty(output))
            {
                _logger.LogDebug("Process output:\n{Output}", output);
            }

            if (!string.IsNullOrEmpty(errors))
            {
                _logger.LogWarning("Process errors:\n{Errors}", errors);
            }

            if (process.ExitCode == 0)
            {
                return true;
            }

            _logger.LogError("Compiler Output:\n{Output}", output);
            _logger.LogError("Compiler Errors:\n{Errors}", errors);
            return false;
        }
    }
}
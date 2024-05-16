using SimpleScriptCompiler.LexicalAnalysis;
using System.Xml;

namespace SimpleScriptCompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // TTODO: Get Path to file from arguments
                string filePath = "ProgramToCompile.simple";
                var tokens = Tokenize(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private static List<Token> Tokenize(string filePath)
        {
            var result = new List<Token>();
            if (!File.Exists(filePath))
            {
                throw new Exception("File not found");
            }

            var lexer = new Lexer();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                int currentLineNumber = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    var tokensForLine = lexer.ConvertToTokens(line, currentLineNumber);
                    result.AddRange(tokensForLine);
                    currentLineNumber++;
                }
            }

            return result;
        }
    }
}

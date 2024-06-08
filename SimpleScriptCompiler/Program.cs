using EntertainingErrors;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScriptCompiler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Result.Success();
                // TTODO: Get Path to file from arguments
                string filePath = "ProgramToCompile.simple";
                List<Token> tokens = Tokenize(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private static List<Token> Tokenize(string filePath)
        {
            List<Token> result = [];
            if (!File.Exists(filePath))
            {
                throw new Exception("File not found");
            }

            Lexer lexer = new();
            using (StreamReader reader = new(filePath))
            {
                string? line;
                int currentLineNumber = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    List<Token> tokensForLine = lexer.ConvertToTokens(line, currentLineNumber);
                    result.AddRange(tokensForLine);
                    currentLineNumber++;
                }
            }

            return result;
        }
    }
}

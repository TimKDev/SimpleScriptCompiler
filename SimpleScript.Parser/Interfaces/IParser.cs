using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Interfaces
{
    public interface IParser
    {
        Result<ProgramNode> ParseTokens(List<Token> inputTokens);
    }
}

using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IReturnNodeFactory
    {
        Result<ReturnNode> Create(List<Token> inputTokens);
    }
}
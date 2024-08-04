using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IInputNodeFactory
    {
        Result<InputNode> Create(List<Token> inputTokens);
    }
}
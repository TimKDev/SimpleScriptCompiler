using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IBodyNodeFactory
    {
        Result<BodyNode> Create(List<Token> inputTokens);
    }
}
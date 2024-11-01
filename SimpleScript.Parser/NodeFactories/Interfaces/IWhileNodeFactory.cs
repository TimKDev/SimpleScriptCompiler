using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces;

public interface IWhileNodeFactory
{
    Result<WhileNode> Create(List<Token> inputTokens, IBodyNodeFactory bodyNodeFactory);
}
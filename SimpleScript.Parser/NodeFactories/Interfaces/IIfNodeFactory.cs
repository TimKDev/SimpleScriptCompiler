using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces;

public interface IIfNodeFactory
{
    Result<IfNode> Create(List<Token> inputTokens, IBodyNodeFactory bodyNodeFactory);
}
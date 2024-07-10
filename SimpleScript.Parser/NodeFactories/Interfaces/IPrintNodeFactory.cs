using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IPrintNodeFactory
    {
        Result<PrintNode> Create(List<Token> inputTokens);
    }
}
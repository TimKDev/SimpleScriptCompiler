using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Interfaces
{
    public interface IFunctionNodeFactory
    {
        Result<FunctionNode> Create(List<Token> inputTokens);
    }
}
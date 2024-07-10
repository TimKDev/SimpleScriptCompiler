using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces
{
    public interface IVariableAssignmentNodeFactory
    {
        Result<VariableDeclarationNode> Create(List<Token> inputTokens);
    }
}
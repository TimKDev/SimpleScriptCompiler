using EntertainingErrors;
using SimpleScript.Lexer;

namespace SimpleScript.Parser
{
    public interface IStatementCombiner
    {
        Result<List<Statement>> CreateStatements(List<Token> tokens);
    }
}
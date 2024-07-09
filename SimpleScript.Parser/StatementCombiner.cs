using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser
{
    public class StatementCombiner : IStatementCombiner
    {
        private TokenType[] TokensTypesToSplit = [TokenType.LET, TokenType.PRINT];
        public Result<List<Statement>> CreateStatements(List<Token> tokens)
        {
            List<Statement> result = [];
            Statement? statement = null;
            foreach (Token token in tokens)
            {
                if (TokensTypesToSplit.Contains(token.TokenType))
                {
                    if (statement != null)
                    {
                        result.Add(statement);
                    }
                    statement = new();
                }
                if (statement == null)
                {
                    return tokens[0].CreateError("Missing Keyword at Statement start.");
                }
                statement.Tokens.Add(token);
            }
            if (statement != null)
            {
                result.Add(statement);
            }

            return result;
        }
    }
}

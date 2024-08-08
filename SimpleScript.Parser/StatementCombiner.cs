using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;

namespace SimpleScript.Parser
{
    public class StatementCombiner : IStatementCombiner
    {
        private TokenType[] TokensTypesToSplit = [TokenType.LET, TokenType.PRINT, TokenType.INPUT, TokenType.FUNC];
        public Result<List<Statement>> CreateStatements(List<Token> tokens)
        {
            List<Statement> result = [];
            Statement? statement = null;
            bool isCurrentlyInFunction = false;
            foreach (Token token in tokens)
            {
                if (token.TokenType == TokenType.ENDBODY)
                {
                    isCurrentlyInFunction = false;
                }
                if (isCurrentlyInFunction && statement != null)
                {
                    statement.Tokens.Add(token);
                    continue;
                }
                if (TokensTypesToSplit.Contains(token.TokenType))
                {
                    if (statement != null)
                    {
                        result.Add(statement);
                    }
                    statement = new();
                    if (token.TokenType == TokenType.FUNC)
                    {
                        isCurrentlyInFunction = true;
                    }
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

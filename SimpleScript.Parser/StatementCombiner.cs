using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;

namespace SimpleScript.Parser
{
    public class StatementCombiner : IStatementCombiner
    {
        private TokenType[] TokensTypesToSplit =
        [
            TokenType.LET, TokenType.PRINT, TokenType.INPUT, TokenType.FUNC, TokenType.RETURN, TokenType.IF,
            TokenType.WHILE
        ];

        private TokenType[] TokensThatDefineAStatementBlock = [TokenType.IF, TokenType.WHILE, TokenType.FUNC];

        public Result<List<Statement>> CreateStatements(List<Token> tokens)
        {
            List<Statement> result = [];
            Statement? statement = null;
            TokenType? tokenTypeToTerminateCurrentStatement = null;
            bool strictlyAppendToCurrentStatement = false;
            foreach (Token token in tokens)
            {
                if (token.TokenType == tokenTypeToTerminateCurrentStatement)
                {
                    strictlyAppendToCurrentStatement = false;
                }

                if (strictlyAppendToCurrentStatement && statement != null)
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
                    if (TokensThatDefineAStatementBlock.Contains(token.TokenType))
                    {
                        strictlyAppendToCurrentStatement = true;
                        tokenTypeToTerminateCurrentStatement = token.TokenType switch
                        {
                            TokenType.FUNC => TokenType.ENDBODY,
                            TokenType.IF => TokenType.ENDIF,
                            TokenType.WHILE => TokenType.ENDWHILE,
                            _ => throw new Exception(
                                $"No statement end token defined for token type: {token.TokenType}"),
                        };
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
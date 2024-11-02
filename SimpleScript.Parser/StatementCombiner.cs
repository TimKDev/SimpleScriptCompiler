using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;

namespace SimpleScript.Parser
{
    public class StatementCombiner : IStatementCombiner
    {
        private TokenType[] TokensTypesToSplit =
        [
            TokenType.Let, TokenType.Print, TokenType.Input, TokenType.Func, TokenType.Return, TokenType.If,
            TokenType.While
        ];

        private TokenType[] TokensThatDefineAStatementBlock = [TokenType.If, TokenType.While, TokenType.Func];

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
                            TokenType.Func => TokenType.EndBody,
                            TokenType.If => TokenType.Endif,
                            TokenType.While => TokenType.EndWhile,
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
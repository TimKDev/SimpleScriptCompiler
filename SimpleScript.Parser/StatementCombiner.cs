using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;

namespace SimpleScript.Parser
{
    public class StatementCombiner : IStatementCombiner
    {
        private readonly TokenType[] _tokensTypesFollowedByExpression =
        [
            TokenType.Let, TokenType.Print, TokenType.Input
        ];

        private readonly (TokenType StartType, TokenType EndType)[] _tokensThatDefineAStatementBlock =
        [
            (TokenType.If, TokenType.Endif), (TokenType.While, TokenType.EndWhile), (TokenType.Func, TokenType.EndBody)
        ];

        private readonly IExpressionCombiner _expressionCombiner;

        public StatementCombiner(IExpressionCombiner expressionCombiner)
        {
            _expressionCombiner = expressionCombiner;
        }

        public Result<List<Statement>> CreateStatements(List<Token> tokens)
        {
            List<Statement> result = [];
            var currentPosition = 0;
            while (currentPosition < tokens.Count)
            {
                var currentToken = tokens[currentPosition];

                //Check for block
                (TokenType StartType, TokenType EndType)? matchedBlockType =
                    _tokensThatDefineAStatementBlock.Select(b => b.StartType).Contains(currentToken.TokenType)
                        ? _tokensThatDefineAStatementBlock.First(blockToken =>
                            currentToken.TokenType == blockToken.StartType)
                        : null;

                if (matchedBlockType != null)
                {
                    var blockStatement = new Statement();
                    while (currentPosition < tokens.Count)
                    {
                        var tokenOfStatement = tokens[currentPosition];
                        blockStatement.Tokens.Add(tokenOfStatement);
                        currentPosition++;
                        if (tokenOfStatement.TokenType == matchedBlockType.Value.EndType)
                        {
                            break;
                        }
                    }

                    result.Add(blockStatement);
                    continue;
                }

                //Check for Function invocation
                if (currentPosition < tokens.Count - 3)
                {
                    if ((tokens[currentPosition].TokenType, tokens[currentPosition + 1].TokenType,
                            tokens[currentPosition + 2].TokenType) is (TokenType.Variable, TokenType.OpenBracket,
                        TokenType.ClosedBracket))
                    {
                        var newStatement = new Statement();
                        newStatement.Tokens.AddRange([
                            tokens[currentPosition], tokens[currentPosition + 1], tokens[currentPosition + 2]
                        ]);
                        result.Add(newStatement);
                        currentPosition += 3;
                        continue;
                    }
                }

                // Check for Tokens followed by expression
                if (currentPosition + 1 < tokens.Count &&
                    _tokensTypesFollowedByExpression.Contains(tokens[currentPosition].TokenType))
                {
                    var expressionTokens = _expressionCombiner.GetExpression(tokens, currentPosition + 1);
                    if (!expressionTokens.IsSuccess)
                    {
                        return Error.Create("End of expression could not been evaluated.");
                    }

                    var newStatement = new Statement();
                    newStatement.Tokens = [tokens[currentPosition], ..expressionTokens.Value];
                    result.Add(newStatement);
                    currentPosition += 1 + expressionTokens.Value.Count;
                    continue;
                }

                return Error.Create("Code could not be parsed into valid statements.");
            }

            return result;
        }
    }
}
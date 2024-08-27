using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories
{
    public class FunctionInvocationNodeFactory : IFunctionInvocationNodeFactory
    {
        public Result<FunctionInvocationNode> Create(List<Token> tokens, IExpressionFactory expressionFactory)
        {
            if (!tokens.Any())
            {
                throw new Exception("No Arguments passed to FunctionInvocationNode Factory.");
            }
            if (tokens is not [{ TokenType: TokenType.Variable, Value: var functionName }, { TokenType: TokenType.OPEN_BRACKET }, .., { TokenType: TokenType.CLOSED_BRACKET }] || functionName is null)
            {
                return tokens.First().CreateError("Invalid function invocation.", tokens.Last().Line);
            }

            Result<IExpression[]> argumentExpressionsResult = ConstructArguments(tokens, expressionFactory);

            return argumentExpressionsResult.IsSuccess ? new FunctionInvocationNode(functionName, argumentExpressionsResult.Value, tokens.First().Line, tokens.Last().Line) : argumentExpressionsResult.Errors;
        }

        private Result<IExpression[]> ConstructArguments(List<Token> tokens, IExpressionFactory expressionFactory)
        {
            Token[] argumentExpressionTokens = tokens.Skip(2).Take(tokens.Count - 3).ToArray();
            List<IExpression> argumentExpressions = [];
            List<Token> expressionParts = [];
            foreach (Token? argumentToken in argumentExpressionTokens)
            {
                if (argumentToken.TokenType == TokenType.COMMA)
                {
                    //Create Expression
                    Result<IExpression> expressionResult = expressionFactory.Create(expressionParts);
                    if (!expressionResult.IsSuccess)
                    {
                        return expressionResult.Errors;
                    }
                    argumentExpressions.Add(expressionResult.Value);
                    expressionParts = [];
                    continue;
                }
                expressionParts.Add(argumentToken);
            }
            if (expressionParts.Any())
            {
                Result<IExpression> lastExpressionResult = expressionFactory.Create(expressionParts);
                if (!lastExpressionResult.IsSuccess)
                {
                    return lastExpressionResult.Errors;
                }

                argumentExpressions.Add(lastExpressionResult.Value);
            }

            return argumentExpressions.ToArray();
        }
    }
}

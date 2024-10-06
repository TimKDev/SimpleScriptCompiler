using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories
{
    public class ReturnNodeFactory : IReturnNodeFactory
    {
        private readonly IExpressionFactory _expressionFactory;

        public ReturnNodeFactory(IExpressionFactory expressionFactory)
        {
            this._expressionFactory = expressionFactory;
        }

        public Result<ReturnNode> Create(List<Token> inputTokens)
        {
            if (!inputTokens.Any())
            {
                return Error.Create("No Return Tokens.");
            }

            if (inputTokens.Count == 1)
            {
                return inputTokens[0]
                    .CreateError("Invalid usage of Return keyword. Return should be followed by expression.");
            }

            List<Token> tokensOfExpression = inputTokens.Skip(1).ToList();
            Result<IExpression> returnExpression = _expressionFactory.Create(tokensOfExpression);

            return returnExpression.IsSuccess
                ? ReturnNode.Create(returnExpression.Value)
                : returnExpression.Errors;
        }
    }
}
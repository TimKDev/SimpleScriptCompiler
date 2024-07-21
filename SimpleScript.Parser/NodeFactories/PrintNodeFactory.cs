using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories
{
    public class PrintNodeFactory : IPrintNodeFactory
    {
        private IExpressionFactory _expressionFactory;

        public PrintNodeFactory(IExpressionFactory expressionFactory)
        {
            _expressionFactory = expressionFactory;
        }

        public Result<PrintNode> Create(List<Token> inputTokens)
        {
            if (!inputTokens.Any())
            {
                return Error.Create("No Print Tokens.");
            }

            if (inputTokens.Count == 1)
            {
                return inputTokens[0].CreateError("Invalid usage of Print keyword. Print should be followed by expression to print.");
            }

            List<Token> tokensOfExpression = inputTokens.Skip(1).ToList();
            Result<IExpression> printExpression = _expressionFactory.Create(tokensOfExpression);

            return printExpression.IsSuccess ? PrintNode.Create(printExpression.Value) : printExpression.Errors;
        }
    }
}

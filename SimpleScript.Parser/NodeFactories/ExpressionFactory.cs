using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser.NodeFactories
{
    public class ExpressionFactory : IExpressionFactory
    {
        private readonly IAdditionNodeFactory _additionNodeFactory;
        private readonly IMultiplicationNodeFactory _multiplicationNodeFactory;

        public ExpressionFactory(IAdditionNodeFactory additionNodeFactory, IMultiplicationNodeFactory multiplicationNodeFactory)
        {
            _additionNodeFactory = additionNodeFactory;
            _multiplicationNodeFactory = multiplicationNodeFactory;
        }

        public Result<IExpression> Create(List<Token> inputTokens)
        {
            int positionOfNextBinaryExpression = FindIndexOfNextBinaryOperator(inputTokens);
            if (positionOfNextBinaryExpression == 0 || positionOfNextBinaryExpression == inputTokens.Count - 1)
            {
                return Error.Create("Binary Operation is missing operant.");
            }

            Token operantToken = inputTokens[positionOfNextBinaryExpression];
            List<Token> firstOperant = inputTokens.Take(positionOfNextBinaryExpression).ToList();
            List<Token> secondOperant = inputTokens.Skip(positionOfNextBinaryExpression + 1).ToList();

            if (operantToken.TokenType == TokenType.PLUS)
            {
                return _additionNodeFactory.Create(firstOperant, secondOperant).Convert<IExpression>();
            }
            else if (operantToken.TokenType == TokenType.MULTIPLY)
            {
                return _multiplicationNodeFactory.Create(firstOperant, secondOperant).Convert<IExpression>();
            }

            return Error.Create("Unknown Error happend.");
        }

        private static int FindIndexOfNextBinaryOperator(List<Token> inputTokens)
        {
            int indexOfNextMultiplication = inputTokens.FindIndex(token => token.TokenType == TokenType.MULTIPLY);
            int indexOfNextAddition = inputTokens.FindIndex(token => token.TokenType == TokenType.PLUS);

            return indexOfNextAddition != -1 ? indexOfNextAddition : indexOfNextMultiplication;
        }
    }
}

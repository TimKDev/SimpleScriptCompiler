using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

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
            if (!inputTokens.Any())
            {
                Error.Create("Expression is empty.");
            }
            int positionOfNextBinaryExpression = FindIndexOfNextBinaryOperator(inputTokens);

            if (positionOfNextBinaryExpression == -1)
            {
                return TransformSingleTokenToExpression(inputTokens[0]);
            }

            if (positionOfNextBinaryExpression == 0 || positionOfNextBinaryExpression == inputTokens.Count - 1)
            {
                return inputTokens[positionOfNextBinaryExpression].CreateError("Binary Operation is missing operant.");
            }

            Token operantToken = inputTokens[positionOfNextBinaryExpression];
            List<Token> firstOperant = inputTokens.Take(positionOfNextBinaryExpression).ToList();
            List<Token> secondOperant = inputTokens.Skip(positionOfNextBinaryExpression + 1).ToList();

            return operantToken.TokenType switch
            {
                TokenType.PLUS => _additionNodeFactory.Create(firstOperant, secondOperant, this).Convert<IExpression>(),
                TokenType.MULTIPLY => _multiplicationNodeFactory.Create(firstOperant, secondOperant, this).Convert<IExpression>(),
                _ => Error.Create("Unknown Error happend.")
            };
        }

        private int FindIndexOfNextBinaryOperator(List<Token> inputTokens)
        {
            int indexOfNextMultiplication = inputTokens.FindIndex(token => token.TokenType == TokenType.MULTIPLY);
            int indexOfNextAddition = inputTokens.FindIndex(token => token.TokenType == TokenType.PLUS);

            return indexOfNextAddition != -1 ? indexOfNextAddition : indexOfNextMultiplication;
        }

        private Result<IExpression> TransformSingleTokenToExpression(Token operand)
        {
            return operand.TokenType switch
            {
                TokenType.String => new StringNode(operand.Value!, operand.Line, operand.Line),
                TokenType.Number => new NumberNode(int.Parse(operand.Value!), operand.Line, operand.Line),
                TokenType.Variable => new VariableNode(operand.Value!, operand.Line, operand.Line),
                _ => Error.Create($"Token type {operand.TokenType} is not supported for addition.")
            };
        }
    }
}

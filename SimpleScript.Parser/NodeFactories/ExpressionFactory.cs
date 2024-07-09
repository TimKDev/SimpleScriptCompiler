using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
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

            if (positionOfNextBinaryExpression == -1)
            {
                return TransformTokenToExpressionNode(inputTokens[0]);
            }

            if (positionOfNextBinaryExpression == 0 || positionOfNextBinaryExpression == inputTokens.Count - 1)
            {
                return inputTokens[positionOfNextBinaryExpression].CreateError("Binary Operation is missing operant.");
            }

            Token operantToken = inputTokens[positionOfNextBinaryExpression];
            List<Token> firstOperant = inputTokens.Take(positionOfNextBinaryExpression).ToList();
            List<Token> secondOperant = inputTokens.Skip(positionOfNextBinaryExpression + 1).ToList();

            if (operantToken.TokenType == TokenType.PLUS)
            {
                return _additionNodeFactory.Create(firstOperant, secondOperant, this).Convert<IExpression>();
            }
            else if (operantToken.TokenType == TokenType.MULTIPLY)
            {
                return _multiplicationNodeFactory.Create(firstOperant, secondOperant, this).Convert<IExpression>();
            }

            return Error.Create("Unknown Error happend.");
        }

        private int FindIndexOfNextBinaryOperator(List<Token> inputTokens)
        {
            int indexOfNextMultiplication = inputTokens.FindIndex(token => token.TokenType == TokenType.MULTIPLY);
            int indexOfNextAddition = inputTokens.FindIndex(token => token.TokenType == TokenType.PLUS);

            return indexOfNextAddition != -1 ? indexOfNextAddition : indexOfNextMultiplication;
        }

        private Result<IExpression> TransformTokenToExpressionNode(Token operand)
        {
            return operand.TokenType switch
            {
                TokenType.String => new StringNode(operand.Value!),
                TokenType.Number => new NumberNode(int.Parse(operand.Value!)),
                TokenType.Variable => new VariableNode(operand.Value!),
                _ => Error.Create($"Token type {operand.TokenType} is not supported for addition.")
            };
        }
    }
}

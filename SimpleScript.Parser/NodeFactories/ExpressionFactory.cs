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
        private readonly IFunctionInvocationNodeFactory _functionInvocationNodeFactory;

        public ExpressionFactory(IAdditionNodeFactory additionNodeFactory, IMultiplicationNodeFactory multiplicationNodeFactory, IFunctionInvocationNodeFactory functionInvocationNodeFactory)
        {
            _additionNodeFactory = additionNodeFactory;
            _multiplicationNodeFactory = multiplicationNodeFactory;
            _functionInvocationNodeFactory = functionInvocationNodeFactory;
        }

        public Result<IExpression> Create(List<Token> inputTokens)
        {
            if (!inputTokens.Any())
            {
                return Error.Create("Expression is empty.");
            }

            if (inputTokens is [{ TokenType: TokenType.Variable }, { TokenType: TokenType.OPEN_BRACKET }, .., { TokenType: TokenType.CLOSED_BRACKET }])
            {
                return (_functionInvocationNodeFactory.Create(inputTokens, this)).Convert<IExpression>();
            }

            int positionOfNextBinaryExpression = FindIndexOfOperationWithSmallestSpeficity(inputTokens);

            if (positionOfNextBinaryExpression == -1)
            {
                return TransformSingleTokenToExpression(inputTokens[0]);
            }

            if (positionOfNextBinaryExpression == 0 || positionOfNextBinaryExpression == inputTokens.Count - 1)
            {
                return inputTokens[positionOfNextBinaryExpression].CreateError("Binary Operation is missing operant.");
            }

            Token operantToken = inputTokens[positionOfNextBinaryExpression];
            List<Token> firstOperant = RemoveRedundantBrackets(inputTokens.Take(positionOfNextBinaryExpression).ToList());
            List<Token> secondOperant = RemoveRedundantBrackets(inputTokens.Skip(positionOfNextBinaryExpression + 1).ToList());

            return operantToken.TokenType switch
            {
                TokenType.PLUS => _additionNodeFactory.Create(firstOperant, secondOperant, this).Convert<IExpression>(),
                TokenType.MULTIPLY => _multiplicationNodeFactory.Create(firstOperant, secondOperant, this).Convert<IExpression>(),
                _ => Error.Create("Unknown Error happend.")
            };
        }

        private int FindIndexOfOperationWithSmallestSpeficity(List<Token> inputTokens)
        {
            int indexOfNextOperation = -1;
            int currentSpecificity = 0;
            int currentLowestSpecificity = int.MaxValue;
            for (int i = 0; i < inputTokens.Count; i++)
            {
                Token currentToken = inputTokens[i];
                if (currentToken.TokenType == TokenType.OPEN_BRACKET)
                {
                    currentSpecificity += 10;
                }
                if (currentToken.TokenType == TokenType.CLOSED_BRACKET)
                {
                    currentSpecificity -= 10;
                }
                if (currentToken.TokenType == TokenType.MULTIPLY && currentSpecificity + 1 < currentLowestSpecificity)
                {
                    indexOfNextOperation = i;
                    currentLowestSpecificity = currentSpecificity + 1;
                }
                if (currentToken.TokenType == TokenType.PLUS && currentSpecificity < currentLowestSpecificity)
                {
                    indexOfNextOperation = i;
                    currentLowestSpecificity = currentSpecificity;
                }
            }

            return indexOfNextOperation;
        }

        private List<Token> RemoveRedundantBrackets(List<Token> inputTokens)
        {
            int numberBracketsAtStart = inputTokens.TakeWhile(token => token.TokenType == TokenType.OPEN_BRACKET).ToList().Count;
            int numberRedundantBrackets = 0;
            for (int i = inputTokens.Count - 1; i >= inputTokens.Count - numberBracketsAtStart; i--)
            {
                if (inputTokens[i].TokenType != TokenType.CLOSED_BRACKET)
                {
                    break;
                }
                numberRedundantBrackets++;
            }

            inputTokens.RemoveRange(0, numberRedundantBrackets);
            inputTokens.RemoveRange(inputTokens.Count - numberRedundantBrackets, numberRedundantBrackets);

            return inputTokens;
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

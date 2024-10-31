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
        private readonly IEqualityNodeFactory _equalityNodeFactory;
        private readonly IInEqualityNodeFactory _inEqualityNodeFactory;
        private readonly IGreaterNodeFactory _greaterNodeFactory;
        private readonly IGreaterOrEqualNodeFactory _greaterOrEqualNodeFactory;
        private readonly ISmallerNodeFactory _smallerNodeFactory;
        private readonly ISmallerOrEqualNodeFactory _smallerOrEqualNodeFactory;

        public ExpressionFactory(IAdditionNodeFactory additionNodeFactory,
            IMultiplicationNodeFactory multiplicationNodeFactory,
            IFunctionInvocationNodeFactory functionInvocationNodeFactory, IEqualityNodeFactory equalityNodeFactory,
            IInEqualityNodeFactory inEqualityNodeFactory, IGreaterNodeFactory greaterNodeFactory,
            IGreaterOrEqualNodeFactory greaterOrEqualNodeFactory, ISmallerNodeFactory smallerNodeFactory,
            ISmallerOrEqualNodeFactory smallerOrEqualNodeFactory)
        {
            _additionNodeFactory = additionNodeFactory;
            _multiplicationNodeFactory = multiplicationNodeFactory;
            _functionInvocationNodeFactory = functionInvocationNodeFactory;
            _equalityNodeFactory = equalityNodeFactory;
            _inEqualityNodeFactory = inEqualityNodeFactory;
            _greaterNodeFactory = greaterNodeFactory;
            _greaterOrEqualNodeFactory = greaterOrEqualNodeFactory;
            _smallerNodeFactory = smallerNodeFactory;
            _smallerOrEqualNodeFactory = smallerOrEqualNodeFactory;
        }

        public Result<IExpression> Create(List<Token> inputTokens)
        {
            if (!inputTokens.Any())
            {
                return Error.Create("Expression is empty.");
            }

            if (inputTokens.Count(token => token.TokenType == TokenType.OPEN_BRACKET) !=
                inputTokens.Count(token => token.TokenType == TokenType.CLOSED_BRACKET))
            {
                return inputTokens.First().CreateError("Number of Brackets are not equal", inputTokens.Last().Line);
            }

            if (inputTokens is
                [
                    { TokenType: TokenType.Variable }, { TokenType: TokenType.OPEN_BRACKET }, ..,
                    { TokenType: TokenType.CLOSED_BRACKET }
                ])
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
            List<Token> firstOperant =
                RemoveRedundantBrackets(inputTokens.Take(positionOfNextBinaryExpression).ToList());
            List<Token> secondOperant =
                RemoveRedundantBrackets(inputTokens.Skip(positionOfNextBinaryExpression + 1).ToList());

            return operantToken.TokenType switch
            {
                TokenType.PLUS => _additionNodeFactory.Create(firstOperant, secondOperant, this).Convert<IExpression>(),
                TokenType.MULTIPLY => _multiplicationNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.EQUAL => _equalityNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.NOTEQUAL => _inEqualityNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.GREATER => _greaterNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.GREATER_OR_EQUAL => _greaterOrEqualNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.SMALLER => _smallerNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.SMALLER_OR_EQUAL => _smallerOrEqualNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                _ => Error.Create("Unknown operation type.")
            };
        }

        private int FindIndexOfOperationWithSmallestSpeficity(List<Token> inputTokens)
        {
            List<TokenType> operationsWithPlusSpecificity =
            [
                TokenType.PLUS, TokenType.EQUAL, TokenType.NOTEQUAL, TokenType.GREATER, TokenType.GREATER_OR_EQUAL,
                TokenType.SMALLER, TokenType.SMALLER_OR_EQUAL
            ];
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

                if (operationsWithPlusSpecificity.Contains(currentToken.TokenType) &&
                    currentSpecificity < currentLowestSpecificity)
                {
                    indexOfNextOperation = i;
                    currentLowestSpecificity = currentSpecificity;
                }
            }

            return indexOfNextOperation;
        }

        private List<Token> RemoveRedundantBrackets(List<Token> inputTokens)
        {
            int numberBracketsAtStart =
                inputTokens.TakeWhile(token => token.TokenType == TokenType.OPEN_BRACKET).ToList().Count;
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
                TokenType.TRUE => new BooleanNode(true, operand.Line, operand.Line),
                TokenType.FALSE => new BooleanNode(false, operand.Line, operand.Line),
                _ => Error.Create($"Token type {operand.TokenType} is not supported for addition.")
            };
        }
    }
}
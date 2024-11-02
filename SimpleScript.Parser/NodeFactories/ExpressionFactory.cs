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
        private readonly IMinusNodeFactory _minusNodeFactory;
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
            ISmallerOrEqualNodeFactory smallerOrEqualNodeFactory, IMinusNodeFactory minusNodeFactory)
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
            _minusNodeFactory = minusNodeFactory;
        }

        public Result<IExpression> Create(List<Token> inputTokens)
        {
            if (!inputTokens.Any())
            {
                return Error.Create("Expression is empty.");
            }

            if (inputTokens.Count(token => token.TokenType == TokenType.OpenBracket) !=
                inputTokens.Count(token => token.TokenType == TokenType.ClosedBracket))
            {
                return inputTokens.First().CreateError("Number of Brackets are not equal", inputTokens.Last().Line);
            }

            if (inputTokens is
                [
                    { TokenType: TokenType.Variable }, { TokenType: TokenType.OpenBracket }, ..,
                    { TokenType: TokenType.ClosedBracket }
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
                TokenType.Plus => _additionNodeFactory.Create(firstOperant, secondOperant, this).Convert<IExpression>(),
                TokenType.Minus => _minusNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.Multiply => _multiplicationNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.Equal => _equalityNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.NotEqual => _inEqualityNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.Greater => _greaterNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.GreaterOrEqual => _greaterOrEqualNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.Smaller => _smallerNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                TokenType.SmallerOrEqual => _smallerOrEqualNodeFactory.Create(firstOperant, secondOperant, this)
                    .Convert<IExpression>(),
                _ => Error.Create("Unknown operation type.")
            };
        }

        private int FindIndexOfOperationWithSmallestSpeficity(List<Token> inputTokens)
        {
            List<TokenType> operationsWithPlusSpecificity =
            [
                TokenType.Plus, TokenType.Minus, TokenType.Equal, TokenType.NotEqual, TokenType.Greater,
                TokenType.GreaterOrEqual,
                TokenType.Smaller, TokenType.SmallerOrEqual
            ];
            int indexOfNextOperation = -1;
            int currentSpecificity = 0;
            int currentLowestSpecificity = int.MaxValue;
            for (int i = 0; i < inputTokens.Count; i++)
            {
                Token currentToken = inputTokens[i];
                if (currentToken.TokenType == TokenType.OpenBracket)
                {
                    currentSpecificity += 10;
                }

                if (currentToken.TokenType == TokenType.ClosedBracket)
                {
                    currentSpecificity -= 10;
                }

                if (currentToken.TokenType == TokenType.Multiply && currentSpecificity + 1 < currentLowestSpecificity)
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
                inputTokens.TakeWhile(token => token.TokenType == TokenType.OpenBracket).ToList().Count;
            int numberRedundantBrackets = 0;
            for (int i = inputTokens.Count - 1; i >= inputTokens.Count - numberBracketsAtStart; i--)
            {
                if (inputTokens[i].TokenType != TokenType.ClosedBracket)
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
                TokenType.True => new BooleanNode(true, operand.Line, operand.Line),
                TokenType.False => new BooleanNode(false, operand.Line, operand.Line),
                _ => Error.Create($"Token type {operand.TokenType} is not supported as a expression.")
            };
        }
    }
}
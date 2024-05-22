using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Interfaces;

namespace SimpleScriptCompiler.SyntaticalAnalysis.Nodes
{
    public class ExpressionNode : INode
    {
        public static TokenType[] SupportedTokenTypes = [
            TokenType.Number,
            TokenType.String,
            TokenType.Variable,
            TokenType.ASSIGN,
            TokenType.PLUS,
            TokenType.MINUS,
            TokenType.MULTIPLY,
            TokenType.DIVIDE,
            TokenType.POWER,
            TokenType.SMALLER,
            TokenType.GREATER,
            TokenType.SMALLER_OR_EQUAL,
            TokenType.GREATER_OR_EQUAL,
            TokenType.EQUAL,
            TokenType.OPEN_BRACKET,
            TokenType.CLOSED_BRACKET
        ];

        public NodeTypes Type => NodeTypes.Expression;

        public IExpressionPart Value { get; }

        private ExpressionNode(IExpressionPart value)
        {
            Value = value;
        }

        // 12 + test * (2**4 -17)
        public static ExpressionNode CreateByTokens(List<Token> tokensOfExpression)
        {
            IExpressionPart expression;

            if (tokensOfExpression.Count == 0)
            {
                throw new ArgumentException("Tokens are needed to construct expression.");
            }

            if (tokensOfExpression.Count == 1)
            {
                expression = CreateValueOrVariableFromToken(tokensOfExpression[0]);
                return new ExpressionNode(expression);
            }
            expression = CreateNextOperationNode(tokensOfExpression);

            return new ExpressionNode(expression);
        }

        private static int? GetIndexOfSmallestSpecificityOperation(List<Token> tokens)
        {
            int smallestSpeficity = 0;
            int? currentSmallestSpeficityIndex = null;
            int currentBracketCount = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (token.TokenType == TokenType.OPEN_BRACKET) currentBracketCount++;
                if (token.TokenType == TokenType.CLOSED_BRACKET) currentBracketCount--;
                if (currentBracketCount < 0) throw new Exception($"Line {token.Line}: Open Bracket must be used before closing one.");

                var currentSpecificity = currentBracketCount * 10;
                if (!OperationNode.SupportedTokenTypes.Contains(token.TokenType)) continue;
                if (token.TokenType == TokenType.MULTIPLY || token.TokenType == TokenType.DIVIDE) currentSpecificity++;
                if (token.TokenType == TokenType.POWER) currentSpecificity += 2;

                if (currentSmallestSpeficityIndex == null || currentSpecificity < smallestSpeficity)
                {
                    currentSmallestSpeficityIndex = i;
                    smallestSpeficity = currentSpecificity;
                }
            }

            if (currentBracketCount > 0)
            {
                throw new Exception("Number of open and closed Brackets must match");
            }

            return currentSmallestSpeficityIndex;
        }

        private static void ComputeASTRecursiv(OperationNode currentOperationNode, List<Token> leftOperantTokens, List<Token> rightOperantTokens)
        {
            //Left Part:
            IExpressionPart? leftExpression;
            if (leftOperantTokens.Count == 1)
            {
                var leftToken = leftOperantTokens[0];
                leftExpression = CreateValueOrVariableFromToken(leftToken);
            }
            else
            {
                leftExpression = CreateNextOperationNode(leftOperantTokens);
            }
            currentOperationNode.FirstOperant = leftExpression ?? throw new Exception();

            //Right Part:
            IExpressionPart? rightExpression;
            if (rightOperantTokens.Count == 1)
            {
                var rightToken = rightOperantTokens[0];
                rightExpression = CreateValueOrVariableFromToken(rightToken);
            }
            else
            {
                rightExpression = CreateNextOperationNode(rightOperantTokens);
            }
            currentOperationNode.SecondOperant = rightExpression ?? throw new Exception();

        }

        private static OperationNode CreateNextOperationNode(List<Token> tokensOfExpression){
            var smallestSpecificityIndex = GetIndexOfSmallestSpecificityOperation(tokensOfExpression) ?? throw new Exception($"Line {tokensOfExpression[0].Line}: Invalid Expression.");
            var tokenWithSmallestSpecificity = tokensOfExpression[smallestSpecificityIndex];
            var leftSide = tokensOfExpression[0..smallestSpecificityIndex];
            var rightSide = tokensOfExpression[(smallestSpecificityIndex + 1)..];
            var currentOperation = OperationNode.Create(TransformTokenType(tokenWithSmallestSpecificity.TokenType));
            ComputeASTRecursiv(currentOperation, leftSide, rightSide);

            return currentOperation;
        }

        private static IExpressionPart CreateValueOrVariableFromToken(Token token)
        {
            return token.TokenType switch
            {
                TokenType.Number => NumberValueNode.CreateFromToken(token),
                TokenType.String => StringValueNode.CreateFromToken(token),
                TokenType.Variable => VariableNode.CreateFromToken(token),
                _ => throw new Exception($"Line {token.Line}: Expression Form invalid")
            };
        }

        private static OperationTypes TransformTokenType(TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.PLUS => OperationTypes.PLUS,
                TokenType.MINUS => OperationTypes.MINUS,
                TokenType.MULTIPLY => OperationTypes.MULTIPLY,
                TokenType.DIVIDE => OperationTypes.DIVIDE,
                TokenType.POWER => OperationTypes.POWER,
                TokenType.SMALLER => OperationTypes.SMALLER,
                TokenType.GREATER => OperationTypes.GREATER,
                TokenType.SMALLER_OR_EQUAL => OperationTypes.SMALLER_OR_EQUAL,
                TokenType.GREATER_OR_EQUAL => OperationTypes.GREATER_OR_EQUAL,
                TokenType.EQUAL => OperationTypes.EQUAL,
                _ => throw new InvalidOperationException("Invalid token type")
            };
        }
    }
}

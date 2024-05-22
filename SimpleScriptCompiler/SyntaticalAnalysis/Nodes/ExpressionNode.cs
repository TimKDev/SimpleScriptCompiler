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
            if (tokensOfExpression.Count == 0)
            {
                throw new ArgumentException("Tokens are needed to construct expression.");
            }

            var expressionPart = CreateExpressionPart(tokensOfExpression);
            return new ExpressionNode(expressionPart);
        }


        // (2 + 4)*(5 + 6)
        private static (int? index, int specificity) GetIndexOfSmallestSpecificityOperation(List<Token> tokens)
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

            return new(currentSmallestSpeficityIndex, smallestSpeficity);
        }

        private static void ComputeASTRecursiv(OperationNode currentOperationNode, List<Token> leftOperantTokens, List<Token> rightOperantTokens)
        {
            currentOperationNode.FirstOperant = CreateExpressionPart(leftOperantTokens) ?? throw new Exception();
            currentOperationNode.SecondOperant = CreateExpressionPart(rightOperantTokens) ?? throw new Exception();
        }

        private static IExpressionPart CreateExpressionPart(List<Token> tokens)
        {
            if (CheckIfExpressionContainsNoOperation(tokens))
            {
                // Expression does not contain any operation but could have multiple parts which can only be unnecessary brackets at start and end, e.g. ((4))
                tokens = RemoveStartAndEndBrackets(tokens);
                if (tokens.Count != 1) throw new Exception("Invalid Operation");
                var token = tokens[0];
                return CreateValueOrVariableFromToken(token);
            }

            return CreateNextOperationNode(tokens);
        }

        private static bool CheckIfExpressionContainsNoOperation(List<Token> tokens)
        {
            foreach (var token in tokens)
            {
                if (OperationNode.SupportedTokenTypes.Contains(token.TokenType)) return false;
            }
            return true;
        }

        private static OperationNode CreateNextOperationNode(List<Token> tokensOfExpression)
        {
            var (smallestSpecificityIndex, smallestSpecificity) = GetIndexOfSmallestSpecificityOperation(tokensOfExpression);
            if (smallestSpecificityIndex == null)
            {
                throw new Exception($"Line {tokensOfExpression[0].Line}: Invalid Expression.");
            }
            var numberOfUnnecessaryBrackets = (smallestSpecificity - smallestSpecificity % 10) / 10;
            if (numberOfUnnecessaryBrackets != 0)
            {
                tokensOfExpression = tokensOfExpression.Skip(numberOfUnnecessaryBrackets).SkipLast(numberOfUnnecessaryBrackets).ToList();
            }
            var smallestIndexAfterRemovingBrackets = smallestSpecificityIndex.Value - numberOfUnnecessaryBrackets;
            var tokenWithSmallestSpecificity = tokensOfExpression[smallestIndexAfterRemovingBrackets];
            var leftSide = tokensOfExpression[0..smallestIndexAfterRemovingBrackets];
            var rightSide = tokensOfExpression[(smallestIndexAfterRemovingBrackets + 1)..];
            var currentOperation = OperationNode.Create(TransformTokenType(tokenWithSmallestSpecificity.TokenType));
            ComputeASTRecursiv(currentOperation, leftSide, rightSide);

            return currentOperation;
        }

        private static List<Token> RemoveStartAndEndBrackets(List<Token> input)
        {
            if (input[0].TokenType != TokenType.OPEN_BRACKET) return input;
            if (input[^1].TokenType != TokenType.CLOSED_BRACKET) throw new Exception("Open Bracket must be closed.");
            var result = input.Skip(1).SkipLast(1).ToList();
            if (result[0].TokenType == TokenType.OPEN_BRACKET) return RemoveStartAndEndBrackets(result); // e.g (((4)))
            return result;
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

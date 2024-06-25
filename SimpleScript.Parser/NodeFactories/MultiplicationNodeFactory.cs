using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser.NodeFactories
{
    public class MultiplicationNodeFactory : IMultiplicationNodeFactory
    {
        public Result<MultiplyNode> Create(List<Token> firstOperand, List<Token> secondOperand, IExpressionFactory expressionFactory)
        {
            Result<IMultiplyable> firstValueResult = TransformTokensToMultiplyableNode(firstOperand, expressionFactory);
            if (!firstValueResult.IsSuccess)
            {
                return firstValueResult.Convert<MultiplyNode>();
            }

            Result<IMultiplyable> secondValueResult = TransformTokensToMultiplyableNode(secondOperand, expressionFactory);
            if (!secondValueResult.IsSuccess)
            {
                return secondValueResult.Convert<MultiplyNode>();
            }

            if (firstOperand.Count == 1 && secondOperand.Count == 1 && !AreTypesCompatibleForMultiplication(firstOperand[0], secondOperand[0], out Error? error) && error != null)
            {
                return error;
            }

            MultiplyNode multiplyNode = new()
            {
                ChildNodes = { firstValueResult.Value, secondValueResult.Value }
            };

            return multiplyNode;
        }

        private Result<IMultiplyable> TransformTokensToMultiplyableNode(List<Token> tokens, IExpressionFactory expressionFactory)
        {
            if (tokens.Count == 1)
            {
                return TransformTokenToMultiplyableNode(tokens[0]);
            }

            return expressionFactory.Create(tokens).Convert<IMultiplyable>();
        }

        private Result<IMultiplyable> TransformTokenToMultiplyableNode(Token token)
        {
            return token.TokenType switch
            {
                TokenType.Number => new NumberNode(int.Parse(token.Value!)),
                TokenType.Variable => new VariableNode(token.Value!),
                _ => token.CreateError($"Token type {token.TokenType} is not supported for multiplication.")
            };
        }

        private bool AreTypesCompatibleForMultiplication(Token firstOperand, Token secondOperand, out Error? error)
        {
            error = null;

            if (firstOperand.TokenType != TokenType.Variable &&
                secondOperand.TokenType != TokenType.Variable &&
                firstOperand.TokenType != secondOperand.TokenType)
            {
                error = firstOperand.CreateError($"Multiplication between types {firstOperand.TokenType} and {secondOperand.TokenType} is not allowed.", secondOperand.Line);
                return false;
            }

            return true;
        }
    }
}

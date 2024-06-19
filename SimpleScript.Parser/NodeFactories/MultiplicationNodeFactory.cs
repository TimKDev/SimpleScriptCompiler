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
        public Result<MultiplyNode> Create(List<Token> firstOperand, List<Token> secondOperand)
        {
            Result<IMultiplyable> firstValueResult = TransformTokensToMultiplyableNode(firstOperand);
            if (!firstValueResult.IsSuccess)
            {
                return firstValueResult.Convert<MultiplyNode>();
            }

            Result<IMultiplyable> secondValueResult = TransformTokensToMultiplyableNode(secondOperand);
            if (!secondValueResult.IsSuccess)
            {
                return secondValueResult.Convert<MultiplyNode>();
            }

            //if (!AreTypesCompatibleForMultiplication(firstOperand, secondOperand, out Error? error) && error != null)
            //{
            //    return error;
            //}

            MultiplyNode multiplyNode = new()
            {
                ChildNodes = { firstValueResult.Value, secondValueResult.Value }
            };

            return multiplyNode;
        }

        private Result<IMultiplyable> TransformTokensToMultiplyableNode(List<Token> tokens)
        {
            if (tokens.Count == 1)
            {
                return TransformTokenToMultiplyableNode(tokens[0]);
            }

            //TTODO Use DI Conatainer:
            ExpressionFactory expressionFactory = new(new AdditionNodeFactory(), new MultiplicationNodeFactory());

            return expressionFactory.Create(tokens).Convert<IMultiplyable>();
        }

        private Result<IMultiplyable> TransformTokenToMultiplyableNode(Token token)
        {
            return token.TokenType switch
            {
                TokenType.Number => new NumberNode(int.Parse(token.Value!)),
                TokenType.Variable => new VariableNode(token.Value!),
                _ => Error.Create($"Token type {token.TokenType} is not supported for multiplication.")
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

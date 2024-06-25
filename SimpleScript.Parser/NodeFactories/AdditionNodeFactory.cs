using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser.NodeFactories
{
    public class AdditionNodeFactory : IAdditionNodeFactory
    {
        public Result<AddNode> Create(List<Token> firstOperand, List<Token> secondOperand, IExpressionFactory expressionFactory)
        {
            Result<IAddable> firstValueResult = TransformTokensToAddableNode(firstOperand, expressionFactory);
            if (!firstValueResult.IsSuccess)
            {
                return firstValueResult.Convert<AddNode>();
            }

            Result<IAddable> secondValueResult = TransformTokensToAddableNode(secondOperand, expressionFactory);
            if (!secondValueResult.IsSuccess)
            {
                return secondValueResult.Convert<AddNode>();
            }

            if (firstOperand.Count == 1 && secondOperand.Count == 1 && !AreTypesCompatibleForAddition(firstOperand[0], secondOperand[0], out Error? error) && error != null)
            {
                return error;
            }

            AddNode addNode = new()
            {
                ChildNodes = { firstValueResult.Value, secondValueResult.Value }
            };

            return addNode;
        }


        private Result<IAddable> TransformTokensToAddableNode(List<Token> tokens, IExpressionFactory expressionFactory)
        {
            if (tokens.Count == 1)
            {
                return TransformTokenToAddableNode(tokens[0]);
            }

            return expressionFactory.Create(tokens).Convert<IAddable>();
        }

        private Result<IAddable> TransformTokenToAddableNode(Token operand)
        {
            return operand.TokenType switch
            {
                TokenType.String => new StringNode(operand.Value!),
                TokenType.Number => new NumberNode(int.Parse(operand.Value!)),
                TokenType.Variable => new VariableNode(operand.Value!),
                _ => Error.Create($"Token type {operand.TokenType} is not supported for addition.")
            };
        }

        private static bool AreTypesCompatibleForAddition(Token firstOperand, Token secondOperand, out Error? error)
        {
            error = null;

            if (firstOperand.TokenType != TokenType.Variable &&
                secondOperand.TokenType != TokenType.Variable &&
                firstOperand.TokenType != secondOperand.TokenType)
            {
                error = firstOperand.CreateError($"Addition between types {firstOperand.TokenType} and {secondOperand.TokenType} is not allowed.", secondOperand.Line);
                return false;
            }

            return true;
        }
    }
}

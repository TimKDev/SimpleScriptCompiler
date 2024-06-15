using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser.NodeFactories
{
    public static class MultiplicationNodeFactory
    {
        public static Result<MultiplyNode> Create(Token firstOperand, Token secondOperand)
        {
            Result<IMultiplyable> firstValueResult = TransformToMultiplyableNode(firstOperand);
            if (!firstValueResult.IsSuccess)
            {
                return firstValueResult.Convert<MultiplyNode>();
            }

            Result<IMultiplyable> secondValueResult = TransformToMultiplyableNode(secondOperand);
            if (!secondValueResult.IsSuccess)
            {
                return secondValueResult.Convert<MultiplyNode>();
            }

            if (!AreTypesCompatibleForMultiplication(firstOperand, secondOperand, out Error? error) && error != null)
            {
                return error;
            }

            MultiplyNode multiplyNode = new()
            {
                ChildNodes = { firstValueResult.Value, secondValueResult.Value }
            };

            return multiplyNode;
        }

        private static Result<IMultiplyable> TransformToMultiplyableNode(Token operand)
        {
            return operand.TokenType switch
            {
                TokenType.Number => new NumberNode(int.Parse(operand.Value!)),
                TokenType.Variable => new VariableNode(operand.Value!),
                _ => Error.Create($"Token type {operand.TokenType} is not supported for multiplication.")
            };
        }

        private static bool AreTypesCompatibleForMultiplication(Token firstOperand, Token secondOperand, out Error? error)
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

using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser.NodeFactories
{
    public static class AdditionNodeFactory
    {
        public static Result<AddNode> Create(Token firstOperand, Token secondOperand)
        {
            Result<IAddable> firstValueResult = TransformToAddableNode(firstOperand);
            if (!firstValueResult.IsSuccess)
            {
                return firstValueResult.Convert<AddNode>();
            }

            Result<IAddable> secondValueResult = TransformToAddableNode(secondOperand);
            if (!secondValueResult.IsSuccess)
            {
                return secondValueResult.Convert<AddNode>();
            }

            if (!AreTypesCompatibleForAddition(firstOperand, secondOperand, out Error? error) && error != null)
            {
                return error;
            }

            AddNode addNode = new()
            {
                ChildNodes = { firstValueResult.Value, secondValueResult.Value }
            };

            return addNode;
        }

        private static Result<IAddable> TransformToAddableNode(Token operand)
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

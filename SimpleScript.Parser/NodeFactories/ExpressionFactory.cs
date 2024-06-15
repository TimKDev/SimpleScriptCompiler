using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser.NodeFactories
{
    public static class ExpressionFactory
    {
        private static readonly TokenType[] _tokensOfBinaryOperations = [TokenType.PLUS, TokenType.MULTIPLY];

        public static Result<IExpression> Create(List<Token> inputTokens)
        {
            int positionOfBinaryExpression = inputTokens.FindIndex(token => _tokensOfBinaryOperations.Contains(token.TokenType));
            if (positionOfBinaryExpression == 0 || positionOfBinaryExpression == inputTokens.Count - 1)
            {
                return Error.Create("Binary Operation is missing operant.");
            }

            Token operantToken = inputTokens[positionOfBinaryExpression];
            Token firstOperant = inputTokens[positionOfBinaryExpression - 1];
            Token secondOperant = inputTokens[positionOfBinaryExpression + 1];

            if (operantToken.TokenType == TokenType.PLUS)
            {
                return AdditionNodeFactory.Create(firstOperant, secondOperant).Convert<IExpression>();
            }
            else if (operantToken.TokenType == TokenType.MULTIPLY)
            {
                return MultiplicationNodeFactory.Create(firstOperant, secondOperant).Convert<IExpression>();
            }

            return Error.Create("Unknown Error happend.");
        }
    }
}

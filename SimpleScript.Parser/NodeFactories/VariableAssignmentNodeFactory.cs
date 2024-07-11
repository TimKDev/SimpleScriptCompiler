using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser.NodeFactories
{
    public class VariableAssignmentNodeFactory : IVariableAssignmentNodeFactory
    {
        private IExpressionFactory _expressionFactory;

        public VariableAssignmentNodeFactory(IExpressionFactory expressionFactory)
        {
            _expressionFactory = expressionFactory;
        }

        public Result<VariableDeclarationNode> Create(List<Token> inputTokens)
        {
            if (inputTokens.Count < 2 || inputTokens[1].TokenType != TokenType.Variable)
            {
                return inputTokens[0].CreateError("Invalid usage of Let keyword. Let should be followed by a variable name.");
            }
            string? variableName = inputTokens[1].Value;
            if (variableName == null)
            {
                return inputTokens[0].CreateError("Invalid usage of Let keyword. Let should be followed by a variable name not equals to null.");
            }

            return inputTokens.Count > 2 && inputTokens[2].TokenType == TokenType.ASSIGN ? CreateVariableDeklarationWithInitialValue(inputTokens, variableName) : new VariableDeclarationNode(variableName);
        }

        private Result<VariableDeclarationNode> CreateVariableDeklarationWithInitialValue(List<Token> inputTokens, string variableName)
        {
            if (inputTokens.Count < 4)
            {
                return inputTokens[2].CreateError("Missing Assert Value: No value given after the assert symbol.");
            }
            List<Token> tokensOfExpression = inputTokens.Skip(3).ToList();
            Result<IExpression> initialValueExpression = _expressionFactory.Create(tokensOfExpression);
            if (!initialValueExpression.IsSuccess)
            {
                string errorMessage = $"Invalid Expression: {string.Join(", ", initialValueExpression.Errors.Select(e => e.Message))}";
                return inputTokens[2].CreateError(errorMessage);
            }

            return new VariableDeclarationNode(variableName, initialValueExpression.Value);
        }
    }
}

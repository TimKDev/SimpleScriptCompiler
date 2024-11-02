using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories
{
    public class VariableDeclarationNodeFactory : IVariableDeclarartionNodeFactory
    {
        private static readonly string NoVariableNameAfterLetErrorMessage =
            "Invalid usage of Let keyword. Let should be followed by a variable name and an initial value.";

        private static readonly string InvalidVariableNameAfterLetErrorMessage =
            "Invalid usage of Let keyword. Let should be followed by a variable name not equals to null.";

        private static readonly string NoInitialValueErrorMessage =
            "Invalid usage of Let keyword. Let should be followed by a assign to define an initial value.";

        private static readonly string NoValueAfterAssertErrorMessage =
            "Missing Assert Value: No value given after the assert symbol.";

        private static readonly string UnknownErrorMessage = "Unknown Error occured.";

        private readonly IExpressionFactory _expressionFactory;

        public VariableDeclarationNodeFactory(IExpressionFactory expressionFactory)
        {
            _expressionFactory = expressionFactory;
        }

        public Result<VariableDeclarationNode> Create(List<Token> inputTokens) => inputTokens switch
        {
            [
                { TokenType: TokenType.Let }, { TokenType: TokenType.Variable, Value: var variableName },
                { TokenType: TokenType.Assign } assignToken, .. var initialValueExpression
            ] when variableName is not null && initialValueExpression.Count > 0 => CreateWithInitialValue(variableName,
                initialValueExpression, assignToken),

            [
                { TokenType: TokenType.Let, Line: var line },
                { TokenType: TokenType.Variable, Value: var variableName }, not { TokenType: TokenType.Assign }, ..
            ] when variableName is not null => Token.CreateError(NoInitialValueErrorMessage, line),

            [{ TokenType: TokenType.Let, Line: var line }, { TokenType: TokenType.Variable, Value: var variableName }]
                when variableName is not null => Token.CreateError(NoInitialValueErrorMessage, line),

            [{ TokenType: TokenType.Let, Line: var line }] => Token.CreateError(NoVariableNameAfterLetErrorMessage,
                line),

            [{ TokenType: TokenType.Let }, { TokenType: not TokenType.Variable, Line: var line }, ..] =>
                Token.CreateError(NoVariableNameAfterLetErrorMessage, line),

            [{ TokenType: TokenType.Let, Line: var line }, { TokenType: TokenType.Variable, Value: null }, ..] =>
                Token.CreateError(InvalidVariableNameAfterLetErrorMessage, line),

            [
                { TokenType: TokenType.Let }, { TokenType: TokenType.Variable },
                { TokenType: TokenType.Assign, Line: var line }
            ] => Token.CreateError(NoValueAfterAssertErrorMessage, line),

            _ => throw new Exception(UnknownErrorMessage)
        };


        private Result<VariableDeclarationNode> CreateWithInitialValue(string variableName,
            List<Token> initialValueExpressionTokens, Token assignToken)
        {
            Result<IExpression> initialValueExpression = _expressionFactory.Create(initialValueExpressionTokens);
            if (!initialValueExpression.IsSuccess)
            {
                string errorMessage =
                    $"Invalid Expression: {string.Join(", ", initialValueExpression.Errors.Select(e => e.Message))}";
                return assignToken.CreateError(errorMessage);
            }

            return new VariableDeclarationNode(variableName, initialValueExpression.Value);
        }
    }
}
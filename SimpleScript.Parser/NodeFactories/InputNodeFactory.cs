using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories
{
    public class InputNodeFactory : IInputNodeFactory
    {
        private static readonly string NoVariableNameAfterInputErrorMessage = "Invalid usage of Input keyword. Input should be followed by a variable name.";
        private static readonly string InvalidVariableNameAfterLetErrorMessage = "Invalid usage of Input keyword. Input should be followed by a variable name not equals to null.";
        private static readonly string UnknownErrorMessage = "Unknown Error occured.";

        public Result<InputNode> Create(List<Token> inputTokens) => inputTokens switch
        {
        [{ TokenType: TokenType.INPUT, Line: var startLine }, { TokenType: TokenType.Variable, Value: var variableName, Line: var endLine }] when variableName is not null => new InputNode(variableName, startLine, endLine),

        [{ TokenType: TokenType.INPUT, Line: var line }] => Token.CreateError(NoVariableNameAfterInputErrorMessage, line),

        [{ TokenType: TokenType.INPUT }, { TokenType: not TokenType.Variable, Line: var line }, ..] => Token.CreateError(NoVariableNameAfterInputErrorMessage, line),

        [{ TokenType: TokenType.INPUT, Line: var line }, { TokenType: TokenType.Variable, Value: null }, ..] => Token.CreateError(InvalidVariableNameAfterLetErrorMessage, line),

            _ => throw new Exception(UnknownErrorMessage)

        };
    }
}

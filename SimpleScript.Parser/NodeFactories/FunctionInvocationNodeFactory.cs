using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories
{
    public class FunctionInvocationNodeFactory : IFunctionInvocationNodeFactory
    {
        public static Result<FunctionInvocationNode> Create(List<Token> tokens)
        {
            if (tokens is not [{ TokenType: TokenType.Variable, Value: var functionName }, { TokenType: TokenType.OPEN_BRACKET }, .., { TokenType: TokenType.CLOSED_BRACKET }] || functionName is null)
            {

            }
        }
    }
}

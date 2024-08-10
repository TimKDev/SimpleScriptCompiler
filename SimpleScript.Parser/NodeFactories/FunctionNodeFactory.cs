using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories
{
    public class FunctionNodeFactory : IFunctionNodeFactory
    {
        public Result<FunctionNode> Create(List<Token> inputTokens)
        {
            Token? firstToken = inputTokens.FirstOrDefault();
            if (firstToken is null)
            {
                throw new ArgumentException();
            }
            int startLine = firstToken.Line;
            int endLine = inputTokens.Last().Line;

            List<Token> functionDeclarationPart = inputTokens.TakeWhile(token => token.TokenType != TokenType.BODY).ToList();
            List<Token> bodyDeclarationPart = inputTokens.SkipWhile(token => token.TokenType != TokenType.BODY).ToList();

            if (functionDeclarationPart is not [{ TokenType: TokenType.FUNC }, { TokenType: TokenType.Variable }, { TokenType: TokenType.OPEN_BRACKET }, .., { TokenType: TokenType.CLOSED_BRACKET }])
            {
                return Token.CreateError("Invalid function declaration.", startLine, functionDeclarationPart.Last().Line);
            }

            if (!bodyDeclarationPart.Any())
            {
                return firstToken.CreateError("Missing body declaration for function.");
            }

            if (bodyDeclarationPart is not [{ TokenType: TokenType.BODY }, .., { TokenType: TokenType.ENDBODY }])
            {
                return Token.CreateError("Invalid body declaration.", bodyDeclarationPart.First().Line, bodyDeclarationPart.Last().Line);
            }

            string? functionName = functionDeclarationPart[1].Value;
            List<Token> argumentDefinitionTokens = functionDeclarationPart.Skip(3).Take(functionDeclarationPart.Count - 4).ToList();
            List<Token> bodyDefinitionTokens = bodyDeclarationPart.Skip(1).Take(functionDeclarationPart.Count - 1).ToList();

            if (functionName is null)
            {
                return functionDeclarationPart[1].CreateError("Functionname should not be null");
            }

            Result<List<FunctionArgumentNode>> argumentResult = CreateFunctionArgumentNode(argumentDefinitionTokens);
            if (!argumentResult.IsSuccess)
            {
                return argumentResult.Errors;
            }

            return new FunctionNode(functionName, argumentResult.Value, startLine, endLine);
        }

        private Result<List<FunctionArgumentNode>> CreateFunctionArgumentNode(List<Token> tokens)
        {
            List<FunctionArgumentNode> result = [];
            List<List<Token>> argumentTokenLists = SplitTokensOnComma(tokens);
            foreach (List<Token> argumentTokenList in argumentTokenLists)
            {
                if (argumentTokenList.Count != 2)
                {
                    return tokens[0].CreateError("Invalid argument definition. Multiple arguments should be separated by comma.");
                }

                if (argumentTokenList is not [{ TokenType: TokenType.INTARG } or { TokenType: TokenType.STRINGARG }, { TokenType: TokenType.Variable, Value: var name }] || name is null)
                {
                    return tokens[0].CreateError("Invalid argument definition. Argument must be of type int or string.");
                }

                Token argumentTypeToken = argumentTokenList.First();
                Token argumentNameToken = argumentTokenList.Last();
                ArgumentType argumentType = argumentTypeToken.TokenType == TokenType.INTARG ? ArgumentType.Int : ArgumentType.String;
                string argumentName = argumentNameToken.Value!;

                result.Add(new FunctionArgumentNode(argumentType, argumentName, argumentTypeToken.Line, argumentNameToken.Line));
            }

            return result;
        }

        private static List<List<Token>> SplitTokensOnComma(List<Token> tokens)
        {
            List<List<Token>> result = [];
            if (!tokens.Any())
            {
                return result;
            }

            List<Token> currentTokenList = [];
            foreach (Token token in tokens)
            {
                if (token.TokenType == TokenType.COMMA)
                {
                    result.Add(currentTokenList);
                    currentTokenList = [];
                    continue;
                }
                currentTokenList.Add(token);
            }
            result.Add(currentTokenList);

            return result;
        }
    }
}

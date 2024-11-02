using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class WhileNodeFactory : IWhileNodeFactory
{
    private readonly IExpressionFactory _expressionFactory;

    public WhileNodeFactory(IExpressionFactory expressionFactory)
    {
        _expressionFactory = expressionFactory;
    }

    public Result<WhileNode> Create(List<Token> inputTokens, IBodyNodeFactory bodyNodeFactory)
    {
        Token? firstToken = inputTokens.FirstOrDefault();
        if (firstToken is null)
        {
            throw new ArgumentException();
        }

        int startLine = firstToken.Line;
        List<Token> conditionPart =
            inputTokens.TakeWhile(token => token.TokenType != TokenType.Repeat).ToList();
        List<Token> whileBody = inputTokens.SkipWhile(token => token.TokenType != TokenType.Repeat).ToList();

        if (conditionPart is not
            [
                { TokenType: TokenType.While }, ..
            ])
        {
            return Token.CreateError("Invalid while condition definition.", startLine, conditionPart.Last().Line);
        }

        if (!whileBody.Any())
        {
            return firstToken.CreateError("Missing body declaration for while condition.");
        }

        if (whileBody is not [{ TokenType: TokenType.Repeat }, .., { TokenType: TokenType.EndWhile }])
        {
            return Token.CreateError("Invalid body declaration.", whileBody.First().Line,
                whileBody.Last().Line);
        }

        var conditionDefinitionTokens = conditionPart.Skip(1);
        var whileConditionExpressionsResult = _expressionFactory.Create(conditionDefinitionTokens.ToList());

        var whileBodyTokens = whileBody.Skip(1).SkipLast(1);
        var whileBodyResult = bodyNodeFactory.Create(whileBodyTokens.ToList());

        if (!whileConditionExpressionsResult.IsSuccess || !whileBodyResult.IsSuccess)
        {
            return whileConditionExpressionsResult.Errors.Concat(whileBodyResult.Errors).ToList();
        }

        return WhileNode.Create(whileConditionExpressionsResult.Value, whileBodyResult.Value);
    }
}
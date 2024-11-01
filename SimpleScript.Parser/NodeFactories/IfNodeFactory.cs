using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public class IfNodeFactory : IIfNodeFactory
{
    private readonly IExpressionFactory _expressionFactory;

    public IfNodeFactory(IExpressionFactory expressionFactory)
    {
        this._expressionFactory = expressionFactory;
    }

    public Result<IfNode> Create(List<Token> inputTokens, IBodyNodeFactory bodyNodeFactory)
    {
        Token? firstToken = inputTokens.FirstOrDefault();
        if (firstToken is null)
        {
            throw new ArgumentException();
        }

        int startLine = firstToken.Line;
        List<Token> conditionPart =
            inputTokens.TakeWhile(token => token.TokenType != TokenType.DO).ToList();
        List<Token> ifBody = inputTokens.SkipWhile(token => token.TokenType != TokenType.DO).ToList();

        if (conditionPart is not
            [
                { TokenType: TokenType.IF }, ..
            ])
        {
            return Token.CreateError("Invalid If condition definition.", startLine, conditionPart.Last().Line);
        }

        if (!ifBody.Any())
        {
            return firstToken.CreateError("Missing body declaration for if condition.");
        }

        if (ifBody is not [{ TokenType: TokenType.DO }, .., { TokenType: TokenType.ENDIF }])
        {
            return Token.CreateError("Invalid body declaration.", ifBody.First().Line,
                ifBody.Last().Line);
        }

        var conditionDefinitionTokens = conditionPart.Skip(1);
        var ifConditionExpressionsResult = _expressionFactory.Create(conditionDefinitionTokens.ToList());

        var ifBodyTokens = ifBody.Skip(1).SkipLast(1);
        var ifBodyResult = bodyNodeFactory.Create(ifBodyTokens.ToList());

        if (!ifConditionExpressionsResult.IsSuccess || !ifBodyResult.IsSuccess)
        {
            return ifConditionExpressionsResult.Errors.Concat(ifBodyResult.Errors).ToList();
        }

        return IfNode.Create(ifConditionExpressionsResult.Value, ifBodyResult.Value);
    }
}
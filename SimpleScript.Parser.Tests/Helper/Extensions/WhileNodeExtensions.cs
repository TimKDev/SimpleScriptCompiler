using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions;

public static class WhileNodeExtensions
{
    public static BodyNode AssertWhile<TCondition>(this WhileNode node,
        Action<TCondition>? conditionAssertion = null)
    {
        TCondition condition = TH.ConvertTo<TCondition>(node.Condition);
        conditionAssertion?.Invoke(condition);
        return node.Body;
    }
}
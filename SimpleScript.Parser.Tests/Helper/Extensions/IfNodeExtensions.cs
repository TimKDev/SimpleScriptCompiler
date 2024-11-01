using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions;

internal static class IfNodeExtensions
{
    internal static BodyNode AssertIfCondition<TCondition>(this IfNode node,
        Action<TCondition>? conditionAssertion = null)
    {
        TCondition condition = TH.ConvertTo<TCondition>(node.Condition);
        conditionAssertion?.Invoke(condition);
        return node.Body;
    }
}
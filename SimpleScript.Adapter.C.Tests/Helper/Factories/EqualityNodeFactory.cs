using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories;

internal static class EqualityNodeFactory
{
    public static EqualityNode Create(IEqualizable firstArg, IEqualizable secondArg)
    {
        return EqualityNode.Create(firstArg, secondArg).Value;
    }
}
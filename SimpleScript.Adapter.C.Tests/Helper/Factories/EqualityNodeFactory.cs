using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories;

internal static class EqualityNodeFactory
{
    public static EqualityNode Create(IEqualizable firstArg, IEqualizable secondArg)
    {
        return EqualityNode.Create(firstArg, secondArg).Value;
    }
}

internal static class InEqualityNodeFactory
{
    public static InEqualityNode Create(IEqualizable firstArg, IEqualizable secondArg)
    {
        return InEqualityNode.Create(firstArg, secondArg).Value;
    }
}
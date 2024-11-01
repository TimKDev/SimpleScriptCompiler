using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories;

public static class BooleanNodeFactory
{
    public static BooleanNode Create(bool value, int startLine, int endLine)
    {
        return new BooleanNode(value, startLine, endLine);
    }
}
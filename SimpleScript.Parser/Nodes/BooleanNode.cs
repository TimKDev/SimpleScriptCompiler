using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes;

public class BooleanNode : NodeBase, IEqualizable
{
    public static ValueTypes TypeName = ValueTypes.Boolean;
    public bool Value { get; private set; }

    public BooleanNode(bool value, int startLine, int endLine) : base(startLine, endLine)
    {
        Value = value;
    }
}
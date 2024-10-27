namespace SimpleScript.Parser.Nodes;

public class BooleanNode : NodeBase
{
    public static ValueTypes TypeName = ValueTypes.Boolean;
    public bool Value { get; private set; }

    public BooleanNode(bool value, int startLine, int endLine) : base(startLine, endLine)
    {
    }
}
namespace SimpleScript.Parser.Nodes
{
    public class StringNode : IExpression
    {
        public string Value { get; private set; }

        public StringNode(string value)
        {
            Value = value;
        }
    }
}

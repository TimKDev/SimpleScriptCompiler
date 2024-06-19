namespace SimpleScript.Parser.Nodes
{
    public class StringNode : IExpression
    {
        public string Value { get; set; }

        public StringNode(string value)
        {
            Value = value;
        }
    }
}

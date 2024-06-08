namespace SimpleScript.Parser.Nodes
{
    public class StringNode : IPrintableNode
    {
        public string Value { get; set; }

        public StringNode(string value)
        {
            Value = value;
        }
    }
}

namespace SimpleScript.Parser.Nodes
{
    public class NumberNode : IPrintableNode, IAddable
    {
        public int Value { get; set; }

        public NumberNode(int value)
        {
            Value = value;
        }
    }
}

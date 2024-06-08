namespace SimpleScript.Parser.Nodes
{
    public class AddNode : IPrintableNode
    {
        public List<StringNode> ChildNodes { get; set; } = [];
    }
}
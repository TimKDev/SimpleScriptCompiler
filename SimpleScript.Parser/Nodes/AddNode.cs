namespace SimpleScript.Parser.Nodes
{
    public class AddNode : IPrintableNode
    {
        public List<IAddable> ChildNodes { get; set; } = [];
    }
}
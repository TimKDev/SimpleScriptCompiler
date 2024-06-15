namespace SimpleScript.Parser.Nodes
{
    public class AddNode : IBinaryOperation, IExpression
    {
        public List<IAddable> ChildNodes { get; set; } = [];
    }
}
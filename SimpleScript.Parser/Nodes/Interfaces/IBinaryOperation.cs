namespace SimpleScript.Parser.Nodes.Interfaces
{
    public interface IBinaryOperation<TArgument> : INodeBase
    {
        TArgument FirstArgument { get; }
        TArgument SecondArgument { get; }
    }
}
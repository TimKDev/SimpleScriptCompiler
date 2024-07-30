namespace SimpleScript.Parser.Nodes.Interfaces
{
    public interface IBinaryOperation<TArgument> : IBaseNode
    {
        TArgument FirstArgument { get; }
        TArgument SecondArgument { get; }
    }
}
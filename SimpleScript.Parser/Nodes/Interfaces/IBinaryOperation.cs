namespace SimpleScript.Parser.Nodes.Interfaces
{
    public interface IBinaryOperation<TArgument>
    {
        TArgument FirstArgument { get; }
        TArgument SecondArgument { get; }
    }
}
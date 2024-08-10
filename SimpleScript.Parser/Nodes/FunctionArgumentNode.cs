namespace SimpleScript.Parser.Nodes
{
    public class FunctionArgumentNode : NodeBase
    {
        public ArgumentType ArgumentType { get; }
        public string ArgumentName { get; }
        public FunctionArgumentNode(ArgumentType argumentType, string argumentName, int startLine, int endLine) : base(startLine, endLine)
        {
            ArgumentType = argumentType;
            ArgumentName = argumentName;
        }
    }
}
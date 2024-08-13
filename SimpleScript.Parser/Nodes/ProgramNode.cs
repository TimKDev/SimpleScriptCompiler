namespace SimpleScript.Parser.Nodes
{
    public class ProgramNode
    {
        public BodyNode Body { get; }

        public ProgramNode(BodyNode body)
        {
            Body = body;
        }
    }
}

using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class ProgramNode : IHasBodyNode
    {
        public BodyNode Body { get; }

        public ProgramNode(BodyNode body)
        {
            Body = body;
        }
    }
}

using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class InputNode : NodeBase, IBodyNode
    {
        public static int CharLength => 200;
        public string VariableName { get; private set; }
        public InputNode(string variableName, int startLine, int endLine) : base(startLine, endLine)
        {
            VariableName = variableName;
        }


    }
}

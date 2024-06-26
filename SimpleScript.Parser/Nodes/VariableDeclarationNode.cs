using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class VariableDeclarationNode : IProgramRootNodes
    {
        public string VariableName { get; private set; }
        public VariableDeclarationNode(string variableName)
        {
            VariableName = variableName;
        }
    }
}

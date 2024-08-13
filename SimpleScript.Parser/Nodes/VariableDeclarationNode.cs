using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class VariableDeclarationNode : IBodyNode
    {
        public string VariableName { get; private set; }
        public IExpression InitialValue { get; private set; }
        public VariableDeclarationNode(string variableName, IExpression initialValue)
        {
            VariableName = variableName;
            InitialValue = initialValue;
        }
    }
}

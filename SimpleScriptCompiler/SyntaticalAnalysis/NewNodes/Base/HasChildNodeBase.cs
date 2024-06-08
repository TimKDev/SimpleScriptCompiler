using EntertainingErrors;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Interfaces;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;

namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Base
{
    public abstract class HasChildNodeBase : NodeBase, IHasChildNodes
    {
        private List<INode> _childNodes = [];
        public IReadOnlyList<INode> ChildNodes => _childNodes.AsReadOnly();
        protected abstract List<NodeTypes> SupportedChildNodeTypes { get; }

        protected HasChildNodeBase(int start, int end) : base(start, end)
        {
        }

        public void AddChildNode(INode node)
        {
            _childNodes.Add(node);
            if (node.EndLineNumber > EndLineNumber)
            {
                EndLineNumber = node.EndLineNumber;
            }
        }

        public new Result Validate()
        {
            Result result = ValidateNode();
            foreach (INode node in ChildNodes)
            {
                if (!SupportedChildNodeTypes.Contains(node.NodeType))
                {
                    result.Merge(Error.Create($"NodeType {node.NodeType} is not supported as a child node for {NodeType}"));
                }
            }

            return result;
        }
    }
}

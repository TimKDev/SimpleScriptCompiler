namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Interfaces
{
    public interface IHasChildNodes
    {
        IReadOnlyList<INode> ChildNodes { get; }
        void AddChildNode(INode node);
    }
}

using EntertainingErrors;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Interfaces
{
    public interface INodeFactory
    {
        Result<int> AddNodeToParent(IHasChildNodes node, Token[] tokens, int i);
    }
}

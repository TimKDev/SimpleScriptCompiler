using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class ProgramNodeFactory
    {
        public static ProgramNode Create(IProgramRootNodes[] programRootNodes)
        {
            ProgramNode programNode = new();
            foreach (IProgramRootNodes node in programRootNodes)
            {
                programNode.ChildNodes.Add(node);
            }

            return programNode;
        }
    }
}

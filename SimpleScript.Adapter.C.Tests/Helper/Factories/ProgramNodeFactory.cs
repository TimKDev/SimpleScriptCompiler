using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Adapter.C.Tests.Helper.Factories
{
    internal static class ProgramNodeFactory
    {
        public static ProgramNode Create(IBodyNode[] programRootNodes)
        {
            BodyNode bodyNode = new(programRootNodes.ToList(), 1, 1);

            return new ProgramNode(bodyNode);
        }
    }
}

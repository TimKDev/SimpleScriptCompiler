using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class FunctionNodeExtensions
    {
        public static void Assert(this FunctionNode functionNode, string functionName, List<(ArgumentType ArgumentType, string ArgumentName)> arguments)
        {
            functionNode.Name.Should().Be(functionName);
            functionNode.Arguments.Count.Should().Be(arguments.Count);
            for (int i = 0; i < arguments.Count; i++)
            {
                (ArgumentType ArgumentType, string ArgumentName) argument = arguments[i];
                FunctionArgumentNode functionArguments = functionNode.Arguments[i];
                argument.ArgumentName.Should().Be(functionArguments.ArgumentName);
                argument.ArgumentType.Should().Be(functionArguments.ArgumentType);
            }
        }
    }
}

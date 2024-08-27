using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class FunctionInvocationNodeExtensions
    {
        public static TArg Assert<TArg>(this FunctionInvocationNode functionInvocationNode, string name)
        {
            functionInvocationNode.FunctionName.Should().Be(name);
            functionInvocationNode.FunctionArguments.Should().HaveCount(1);
            return TH.ConvertTo<TArg>(functionInvocationNode.FunctionArguments[0]);
        }

        public static (TArg1, TArg2) Assert<TArg1, TArg2>(this FunctionInvocationNode functionInvocationNode, string name)
        {
            functionInvocationNode.FunctionName.Should().Be(name);
            functionInvocationNode.FunctionArguments.Should().HaveCount(2);
            return (TH.ConvertTo<TArg1>(functionInvocationNode.FunctionArguments[0]), TH.ConvertTo<TArg2>(functionInvocationNode.FunctionArguments[1]));
        }
    }
}

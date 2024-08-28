using FluentAssertions;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Tests.Helper.Extensions
{
    internal static class FunctionInvocationNodeExtensions
    {
        public static void Assert(this FunctionInvocationNode functionInvocationNode, string expectedFunctionName)
        {
            functionInvocationNode.FunctionArguments.Length.Should().Be(0);
            functionInvocationNode.FunctionName.Should().Be(expectedFunctionName);
        }

        public static TArgExpression Assert<TArgExpression>(this FunctionInvocationNode functionInvocationNode, string expectedFunctionName) where TArgExpression : IExpression
        {
            functionInvocationNode.FunctionName.Should().Be(expectedFunctionName);
            functionInvocationNode.FunctionArguments.Length.Should().Be(1);
            return TH.ConvertTo<TArgExpression>(functionInvocationNode.FunctionArguments[0]);
        }

        public static (TArgExpression1, TArgExpression2) Assert<TArgExpression1, TArgExpression2>(this FunctionInvocationNode functionInvocationNode, string expectedFunctionName) where TArgExpression1 : IExpression where TArgExpression2 : IExpression

        {
            functionInvocationNode.FunctionName.Should().Be(expectedFunctionName);
            functionInvocationNode.FunctionArguments.Length.Should().Be(2);
            TArgExpression1 firstArg = TH.ConvertTo<TArgExpression1>(functionInvocationNode.FunctionArguments[0]);
            TArgExpression2 secondArg = TH.ConvertTo<TArgExpression2>(functionInvocationNode.FunctionArguments[1]);

            return (firstArg, secondArg);
        }
    }
}

using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;
using Xunit.Abstractions;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests;

public class WhileCondition(ITestOutputHelper testOutputHelper)
{
    private readonly IParser _sut = ParserFactory.Create();

    [Fact]
    public void WhileConditionWithConstantTrue()
    {
        List<Token> programTokens =
        [
            TF.While(), TF.True(), TF.Repeat(), TF.Print(), TF.Str("Hallo Tim"),
            TF.EndWhile()
        ];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens), testOutputHelper);
        programNode.AssertProgramNode<WhileNode>()
            .AssertWhile<BooleanNode>(boolNode => boolNode.AssertBoolean(true))
            .AssertBody<PrintNode>()
            .AssertPrint<StringNode>()
            .AssertString("Hallo Tim");
    }

    [Fact]
    public void WhileConditionWithEqual()
    {
        List<Token> programTokens =
        [
            TF.While(), TF.Var("name"), TF.Equal(), TF.Str("Tim"), TF.Repeat(), TF.Print(), TF.Str("Hallo Tim"),
            TF.EndWhile()
        ];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
        programNode.AssertProgramNode<WhileNode>()
            .AssertWhile<EqualityNode>(equlityNode =>
            {
                var (variableNode, stringNode) = equlityNode.AssertEquality<VariableNode, StringNode>();
                variableNode.AssertVariable("name");
                stringNode.AssertString("Tim");
            })
            .AssertBody<PrintNode>()
            .AssertPrint<StringNode>()
            .AssertString("Hallo Tim");
    }
}
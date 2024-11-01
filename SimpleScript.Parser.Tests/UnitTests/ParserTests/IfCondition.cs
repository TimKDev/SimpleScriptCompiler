using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;
using Xunit.Abstractions;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests;

public class IfCondition(ITestOutputHelper testOutputHelper)
{
    private readonly IParser _sut = ParserFactory.Create();

    [Fact]
    public void IfConditionWithConstantTrue()
    {
        List<Token> programTokens =
        [
            TF.If(), TF.True(), TF.Do(), TF.Print(), TF.Str("Hallo Tim"),
            TF.EndIf()
        ];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens), testOutputHelper);
        programNode.AssertProgramNode<IfNode>()
            .AssertIfCondition<BooleanNode>(boolNode => boolNode.AssertBoolean(true))
            .AssertBody<PrintNode>()
            .AssertPrint<StringNode>()
            .AssertString("Hallo Tim");
    }

    [Fact]
    public void IfConditionWithEqual()
    {
        List<Token> programTokens =
        [
            TF.If(), TF.Var("name"), TF.Equal(), TF.Str("Tim"), TF.Do(), TF.Print(), TF.Str("Hallo Tim"),
            TF.EndIf()
        ];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
    }
}
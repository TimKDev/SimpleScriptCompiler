using SimpleScript.Adapter.C.Tests.Helper.Extensions;
using SimpleScript.Adapter.C.Tests.Helper.Factories;
using SimpleScript.Parser.Nodes;
using Xunit.Abstractions;

namespace SimpleScript.Adapter.C.Tests.ConverterTests;

public class IfConditionShouldBeConvertedToCCode(ITestOutputHelper testOutputHelper)
{
    private readonly ProgramConverterToC _sut = new();

    [Fact]
    public void GivenIfConditionWithPrint()
    {
        ProgramNode programNode = ProgramNodeFactory.Create([
            IfNodeFactory.Create(BooleanNodeFactory.Create(true, 1, 1), [
                PrintNodeFactory.Create(StringNodeFactory.Create("Hallo World!", 1, 1))
            ], 1, 1)
        ]);

        _sut.AssertConverterToCCode(programNode, [
            "if(true)",
            "{",
            "printf(\"Hallo World!\");",
            "}"
        ]);
    }

    [Fact]
    public void GivenIfConditionWithEqualCondition()
    {
        //TTODO Warum gibt dies keinen Fehler? Die Variable "name" wird doch verwendet, obwohl sie noch nicht im Scope existiert.
        //Das sollte nicht erst Runtime auffallen!
        ProgramNode programNode = ProgramNodeFactory.Create([
            IfNodeFactory.Create(
                EqualityNodeFactory.Create(VariableNodeFactory.Create("name", 1, 1), new StringNode("Tim", 1, 1)),
                [
                    PrintNodeFactory.Create(StringNodeFactory.Create("Hallo Tim!", 1, 1))
                ], 1, 1)
        ]);

        _sut.AssertConverterToCCode(programNode, [
            "if((name == \"Tim\"))",
            "{",
            "printf(\"Hallo Tim!\");",
            "}"
        ]);
    }
}
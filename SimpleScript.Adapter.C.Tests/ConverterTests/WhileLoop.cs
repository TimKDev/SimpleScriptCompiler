using SimpleScript.Adapter.C.Tests.Helper.Extensions;
using SimpleScript.Adapter.C.Tests.Helper.Factories;
using SimpleScript.Parser.Nodes;
using Xunit.Abstractions;

namespace SimpleScript.Adapter.C.Tests.ConverterTests;

public class WhileLoop(ITestOutputHelper testOutputHelper)
{
    private readonly ProgramConverterToC _sut = new();

    [Fact]
    public void GivenConditionWithBoolAndPrint()
    {
        //TTODO generell wäre es schöner hier eine Fluent API zu definieren, um die ProgramNode zu definieren.
        ProgramNode programNode = ProgramNodeFactory.Create([
            WhileNodeFactory.Create(BooleanNodeFactory.Create(true, 1, 1), [
                PrintNodeFactory.Create(StringNodeFactory.Create("Hallo World!", 1, 1))
            ], 1, 1)
        ]);

        _sut.AssertConverterToCCode(programNode, [
            "while(true)",
            "{",
            "printf(\"%s\",\"Hallo World!\");",
            "fflush(stdout);",
            "}"
        ]);
    }

    [Fact]
    public void GivenConditionWithEqual()
    {
        ProgramNode programNode = ProgramNodeFactory.Create([
            VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Testname", 1, 1)),
            WhileNodeFactory.Create(
                EqualityNodeFactory.Create(VariableNodeFactory.Create("name", 1, 1), new StringNode("Tim", 1, 1)),
                [
                    PrintNodeFactory.Create(StringNodeFactory.Create("Hallo Tim!", 1, 1))
                ],
                1, 1)
        ]);

        _sut.AssertConverterToCCode(programNode, [
            "char* name = \"Testname\";",
            "while((name == \"Tim\"))",
            "{",
            "printf(\"%s\",\"Hallo Tim!\");",
            "fflush(stdout);",
            "}"
        ], testOutputHelper: testOutputHelper);
    }
}
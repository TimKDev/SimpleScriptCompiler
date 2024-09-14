using SimpleScript.Adapter.C.Tests.Helper.Extensions;
using SimpleScript.Adapter.C.Tests.Helper.Factories;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.ConverterTests
{
    public class FunctionShouldBeConvertedToCCode
    {
        private readonly ProgramConverterToC _sut = new();

        [Fact]
        public void ShouldConvertFunctionDeclarationToCCode()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                FunctionNodeFactory.Create("add", [FunctionArgumentFactory.Create(ArgumentType.Int, "num_1", 1, 1), FunctionArgumentFactory.Create(ArgumentType.Int, "num_2", 1, 1)], BodyNodeFactory.Create([
                    VariableDeclarationNodeFactory.Create("result", AddNodeFactory.Create(VariableNodeFactory.Create("num_1", 2, 2), VariableNodeFactory.Create("num_2", 2, 2))),
                   ReturnNodeFactory.Create(VariableNodeFactory.Create("result", 3, 3))
                ], 2, 3), 1, 3)
            ]);

            _sut.AssertConverterToCCode(programNode, [], [
                "int add(int num_1, int num_2)",
                "{",
                "int result = num_1 + num_2;",
                "return result;",
                "}"
            ]);
        }
    }
}

using EntertainingErrors;
using FluentAssertions;
using SimpleScript.Adapter.C.Tests.Helper.Extensions;
using SimpleScript.Adapter.C.Tests.Helper.Factories;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.ConverterTests
{
    public class Print
    {
        private readonly ProgramConverterToC _sut = new();

        [Fact]
        public void GivenHelloWorldAst()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                PrintNodeFactory.Create(StringNodeFactory.Create("Hello Simple Script Compiler", 1, 1))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "printf(\"Hello Simple Script Compiler\");"
            ]);
        }

        [Fact]
        public void GivenPrintForNumber()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                PrintNodeFactory.Create(NumberNodeFactory.Create(3, 1, 1))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "printf(\"%d\", 3);"
            ]);
        }

        [Fact]
        public void GivenPrintForVariableWithoutDeklaration_ShouldReturnError()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                PrintNodeFactory.Create(VariableNodeFactory.Create("test", 1, 1))
            ]);

            Result<string> result = _sut.ConvertToCCode(programNode);
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void GivenAddOperationBetweenNumbers()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                PrintNodeFactory.Create(AddNodeFactory.Create(
                    NumberNodeFactory.Create(1, 1, 1),
                    NumberNodeFactory.Create(2, 1, 1)
                ))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "printf(\"%d\", (1 + 2));"
            ]);
        }

        [Fact]
        public void GivenAddOperationBetweenStrings()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                PrintNodeFactory.Create(AddNodeFactory.Create(
                    StringNodeFactory.Create("Hello ", 1, 1),
                    StringNodeFactory.Create("World", 1, 1)
                ))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char temp_1[12];",
                "strcpy(temp_1, \"Hello \");",
                "strcat(temp_1, \"World\");",
                "printf(temp_1);",
            ]);
        }

        [Fact]
        public void GivenAddAndMultiplyOperationBetweenNumbers()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                PrintNodeFactory.Create(AddNodeFactory.Create(
                    MultiplyNodeFactory.Create(
                        NumberNodeFactory.Create(1, 1, 1),
                        NumberNodeFactory.Create(2, 1, 1)
                    ),
                    NumberNodeFactory.Create(3, 1, 1)
                ))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "printf(\"%d\", ((1 * 2) + 3));"
            ]);
        }

        [Fact]
        public void GivenMultiplyAndAddOperationBetweenNumbersWithDist()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                PrintNodeFactory.Create(MultiplyNodeFactory.Create(
                    AddNodeFactory.Create(
                        NumberNodeFactory.Create(1, 1, 1),
                        NumberNodeFactory.Create(2, 1, 1)
                    ),
                    NumberNodeFactory.Create(3, 1, 1)
                ))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "printf(\"%d\", ((1 + 2) * 3));"
            ]);
        }

        [Fact]
        public void GivenExpressionWith2AddAnd1MulOperationBetweenNumbers()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                PrintNodeFactory.Create(MultiplyNodeFactory.Create(
                    AddNodeFactory.Create(
                        NumberNodeFactory.Create(1, 1, 1),
                        NumberNodeFactory.Create(2, 1, 1)
                    ),
                    AddNodeFactory.Create(
                        NumberNodeFactory.Create(3, 1, 1),
                        NumberNodeFactory.Create(4, 1, 1)
                    )
                ))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "printf(\"%d\", ((1 + 2) * (3 + 4)));"
            ]);
        }
    }
}
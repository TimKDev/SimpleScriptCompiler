using SimpleScript.Adapter.C.Tests.Helper.Extensions;
using SimpleScript.Adapter.C.Tests.Helper.Factories;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C.Tests.ConverterTests
{
    public class LetDeklarationShouldConvertToCCode
    {
        private readonly ConverterToCCode _sut = new();

        [Fact]
        public void GivenLetDeklarationWithStringInitialValue()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Tim", 1, 1))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char name[] = \"Tim\";"
            ]);
        }

        [Fact]
        public void GivenLetDeklarationWithNumberInitialValue()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", NumberNodeFactory.Create(42, 1, 1))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "int name = 42;"
            ]);
        }

        [Fact]
        public void GivenLetDeklarationWithAddExpressionInitialValue()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", AddNodeFactory.Create(NumberNodeFactory.Create(3, 1, 1), NumberNodeFactory.Create(4, 1, 1)))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "int name = (3 + 4);"
            ]);
        }

        [Fact]
        public void GivenLetDeklarationWithAddExpressionWithString()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", AddNodeFactory.Create(StringNodeFactory.Create("Hello ", 1, 1), StringNodeFactory.Create("World", 1, 1)))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char name[] = (\"Hello \" + \"World\");"
            ]);
        }

        [Fact]
        public void GivenTwoVariableDeklarationsShouldInferTypeCorrectly()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Tim", 1, 1)),
                VariableDeclarationNodeFactory.Create("message", AddNodeFactory.Create(VariableNodeFactory.Create("name", 2, 2), StringNodeFactory.Create(" ist mein Name", 2, 2)))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char name[] = \"Tim\";",
                "char message[] = (name + \" ist mein Name\");"
            ]);
        }
    }
}

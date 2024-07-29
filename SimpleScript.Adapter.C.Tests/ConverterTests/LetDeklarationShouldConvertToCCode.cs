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
                VariableDeclarationNodeFactory.Create("name", NumberNodeFactory.Create(42, 1, 1))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "int name = 42;"
            ]);
        }
    }
}

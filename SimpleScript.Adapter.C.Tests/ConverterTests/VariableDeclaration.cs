using SimpleScript.Adapter.C.Tests.Helper.Extensions;
using SimpleScript.Adapter.C.Tests.Helper.Factories;
using SimpleScript.Parser.Nodes;
using Xunit.Abstractions;
using VariableDeclarationNodeFactory = SimpleScript.Adapter.C.Tests.Helper.Factories.VariableDeclarationNodeFactory;

namespace SimpleScript.Adapter.C.Tests.ConverterTests
{
    public class VariableDeclaration(ITestOutputHelper testOutputHelper)
    {
        private readonly ProgramConverterToC _sut = new();

        [Fact]
        public void GivenLetDeklarationWithStringInitialValue()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Tim", 1, 1))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char *name = \"Tim\";"
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
                VariableDeclarationNodeFactory.Create("name",
                    AddNodeFactory.Create(NumberNodeFactory.Create(3, 1, 1), NumberNodeFactory.Create(4, 1, 1)))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "int name = (3 + 4);"
            ]);
        }

        [Fact]
        public void GivenLetDeklarationWithAddExpressionWithString()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name",
                    AddNodeFactory.Create(StringNodeFactory.Create("Hello ", 1, 1),
                        StringNodeFactory.Create("World", 1, 1)))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char temp_1[12];",
                "strcpy(temp_1, \"Hello \");",
                "strcat(temp_1, \"World\");",
                "char *name = temp_1;"
            ]);
        }

        [Fact]
        public void GivenTwoVariableDeklarationsShouldInferTypeCorrectly()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Tim", 1, 1)),
                VariableDeclarationNodeFactory.Create("message",
                    AddNodeFactory.Create(VariableNodeFactory.Create("name", 2, 2),
                        StringNodeFactory.Create(" ist mein Name", 2, 2)))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char *name = \"Tim\";",
                "char temp_1[18];",
                "strcpy(temp_1, name);",
                "strcat(temp_1, \" ist mein Name\");",
                "char *message = temp_1;"
            ]);
        }

        [Fact]
        public void GivenDeklarationAndReassign_ForString()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Tim", 1, 1)),
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Caro", 1, 1))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char *name = \"Tim\";",
                "name = \"Caro\";"
            ]);
        }

        [Fact]
        public void GivenDeklarationAndReassign_ForInt()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("age", NumberNodeFactory.Create(12, 1, 1)),
                VariableDeclarationNodeFactory.Create("age", NumberNodeFactory.Create(17, 1, 1))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "int age = 12;",
                "age = 17;"
            ]);
        }


        [Fact]
        public void GivenDeklarationAndReassign_ForStringAddInitialValue()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name",
                    AddNodeFactory.Create(StringNodeFactory.Create("Tim", 2, 2),
                        StringNodeFactory.Create(" ist mein Name", 2, 2))),
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Caro", 1, 1))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char temp_1[18];",
                "strcpy(temp_1, \"Tim\");",
                "strcat(temp_1, \" ist mein Name\");",
                "char *name = temp_1;",
                "name = \"Caro\";"
            ]);
        }

        [Fact]
        public void GivenDeklarationAndReassign_ForStringAddReassignValue()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Tim", 1, 1)),
                VariableDeclarationNodeFactory.Create("name",
                    AddNodeFactory.Create(StringNodeFactory.Create("Tim", 2, 2),
                        StringNodeFactory.Create(" ist mein Name", 2, 2)))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char *name = \"Tim\";",
                "char temp_1[18];",
                "strcpy(temp_1, \"Tim\");",
                "strcat(temp_1, \" ist mein Name\");",
                "name = temp_1;",
            ]);
        }


        [Fact]
        public void GivenDeklarationReassignAndAddition_ForString_ShouldUseNewLengthForTempVariable()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Tim", 1, 1)),
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Carolin", 1, 1)),
                VariableDeclarationNodeFactory.Create("message",
                    AddNodeFactory.Create(VariableNodeFactory.Create("name", 2, 2),
                        StringNodeFactory.Create(" ist mein Name", 2, 2)))
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char *name = \"Tim\";",
                "name = \"Carolin\";",
                "char temp_1[22];",
                "strcpy(temp_1, name);",
                "strcat(temp_1, \" ist mein Name\");",
                "char *message = temp_1;"
            ]);
        }

        [Fact]
        public void GivenBooleanDeclaration_ShouldConvertToCCode()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Carolin", 1, 1)),
                VariableDeclarationNodeFactory.Create("isNotProgrammer",
                    InEqualityNodeFactory.Create(
                        VariableNodeFactory.Create("name", 1, 1),
                        StringNodeFactory.Create("Tim", 1, 1)
                    )
                ),
            ]);

            _sut.AssertConverterToCCode(programNode, [
                "char *name = \"Carolin\";",
                "int isNotProgrammer = (name != \"Tim\");"
            ]);
        }

        [Fact]
        public void GivenBooleanDeclarationWithNotCompatibleEquals_ShouldReturnTypeError()
        {
            ProgramNode programNode = ProgramNodeFactory.Create([
                VariableDeclarationNodeFactory.Create("name", StringNodeFactory.Create("Carolin", 1, 1)),
                VariableDeclarationNodeFactory.Create("isNotProgrammer",
                    InEqualityNodeFactory.Create(
                        VariableNodeFactory.Create("name", 1, 1),
                        NumberNodeFactory.Create(111, 1, 1)
                    )
                ),
            ]);

            _sut.AssertErrorsConverterToCCode(programNode, [
                "Error Line 1: Types String and Number are not compatible.",
            ], testOutputHelper);
        }
    }
}
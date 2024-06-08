using FluentAssertions;
using SimpleScript.Parser.Nodes;
using SimpleScriptCompiler.LexicalAnalysis;
using TF = SimpleScript.Parser.Tests.Helper.TokenFactory;

namespace SimpleScript.Parser.Tests
{
    public class ParserTestsHelloWorldWithPlusProgram
    {
        private readonly Parser _sut = new();
        private static readonly string Hello = "Hello";
        private static readonly string World = "World";
        private readonly List<Token> ProgramTokens = [TF.Print(), TF.Str(Hello), TF.Add(), TF.Str(World)];

        [Fact]
        public void ParserTests_PrintNodeShouldHaveAddNodeAsChild_GivenProgramTokens()
        {
            ProgramNode programmingNode = _sut.ParseTokens(ProgramTokens);
            programmingNode.ChildNodes.Count.Should().Be(1);
            programmingNode.ChildNodes[0].Should().BeOfType<PrintNode>();
            PrintNode printNode = programmingNode.ChildNodes[0];
            printNode.ChildNodes.Count().Should().Be(1);
            printNode.ChildNodes[0].Should().BeOfType<AddNode>();
        }

        [Fact]
        public void ParserTests_AddNodeShouldHaveTwoStringsAsChildren()
        {
            ProgramNode programmingNode = _sut.ParseTokens(ProgramTokens);
            PrintNode printNode = programmingNode.ChildNodes[0];
            AddNode? addNode = printNode.ChildNodes[0] as AddNode;
            addNode!.ChildNodes.Count.Should().Be(2);
        }

        [Fact]
        public void ParserTests_StringsShouldHaveValuesHelloAndWorld()
        {
            ProgramNode programmingNode = _sut.ParseTokens(ProgramTokens);
            PrintNode printNode = programmingNode.ChildNodes[0];
            AddNode addNode = (printNode.ChildNodes[0] as AddNode)!;
            StringNode firstString = addNode.ChildNodes[0];
            StringNode secondString = addNode.ChildNodes[1];
            firstString.Value.Should().Be(Hello);
            secondString.Value.Should().Be(World);
        }
    }
}

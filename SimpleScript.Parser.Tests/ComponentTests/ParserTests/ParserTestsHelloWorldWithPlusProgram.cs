using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper;
using SimpleScript.Parser.Tests.Helper.Factories;
using TF = SimpleScript.Parser.Tests.Helper.Factories.TokenFactory;

namespace SimpleScript.Parser.Tests.ComponentTests.ParserTests
{
    public class ParserTestsHelloWorldWithPlusProgram
    {
        private static readonly string Hello = "Hello";
        private static readonly string World = "World";
        private readonly List<Token> ProgramTokens = [TF.Print(), TF.Str(Hello), TF.Add(), TF.Str(World)];

        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParserTests_PrintNodeShouldHaveAddNodeAsChild_GivenProgramTokens()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            programmingNode.ChildNodes.Count.Should().Be(1);
            programmingNode.ChildNodes[0].Should().BeOfType<PrintNode>();
            PrintNode printNode = programmingNode.ChildNodes[0];
            printNode.ChildNodes.Count().Should().Be(1);
            printNode.ChildNodes[0].Should().BeOfType<AddNode>();
        }

        [Fact]
        public void ParserTests_AddNodeShouldHaveTwoStringsAsChildren()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = programmingNode.ChildNodes[0];
            AddNode? addNode = printNode.ChildNodes[0] as AddNode;
            addNode!.ChildNodes.Count.Should().Be(2);
        }

        [Fact]
        public void ParserTests_StringsShouldHaveValuesHelloAndWorld()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = programmingNode.ChildNodes[0];
            AddNode addNode = (printNode.ChildNodes[0] as AddNode)!;
            StringNode? firstString = addNode.ChildNodes[0] as StringNode;
            StringNode? secondString = addNode.ChildNodes[1] as StringNode;
            firstString!.Value.Should().Be(Hello);
            secondString!.Value.Should().Be(World);
        }
    }
}

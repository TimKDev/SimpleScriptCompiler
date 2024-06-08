using FluentAssertions;
using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Factories;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Nodes;
using TF = TokenFactory;

namespace SimpleScriptCompiler.Tests.SyntacticalAnalysis.Nodes.Factories
{
    public class AdditionNodeFactoryTests
    {
        private readonly AdditionNodeFactory _sut = new();

        [Fact]
        public void AddNodeToParent_ShouldAddASimpleAddition_Given2Plus3()
        {
            ExpressionNode parentNode = new(1, 1);
            List<Token> leftTokens = [TF.Num(2)];
            List<Token> rightTokens = [TF.Num(3)];
            EntertainingErrors.Result<int> result = _sut.AddNodeToParent(parentNode, leftTokens, rightTokens);
            parentNode.ChildNodes.Count.Should().Be(1);
            parentNode.ChildNodes[0].Should().BeOfType<AdditionNode>();
            //var additionNode = (parentNode.ChildNodes[0] as AdditionNode)!;
        }
    }
}

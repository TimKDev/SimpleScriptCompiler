using FluentAssertions;
using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;
using tf = TokenFactory;

namespace SimpleScriptCompiler.Tests.Nodes
{
    public class ExpressionNodeTests
    {
        [Fact]
        public void CreateByTokens_ShouldConstructExpression_GivenSimpleAddition()
        {
            //1 + test
            var input = new List<Token>() { tf.Num(1), tf.Add(), tf.Var("test") };
            var result = ExpressionNode.CreateByTokens(input);
            result.Value.Should().BeOfType<OperationNode>();
            OperationNode operationNode = (OperationNode)result.Value;
            operationNode.OperationType.Should().Be(OperationTypes.PLUS);
            operationNode.FirstOperant.Should().BeOfType<NumberValueNode>();
            operationNode.SecondOperant.Should().BeOfType<VariableNode>();

        }

        [Fact]
        public void CreateByTokens_ShouldConstructExpression_GivenSimpleMultiplication()
        {
            //1 * 7
            var input = new List<Token>() { tf.Num(1), tf.Mul(), tf.Num(7) };
            var result = ExpressionNode.CreateByTokens(input);
            result.Value.Should().BeOfType<OperationNode>();
            OperationNode operationNode = (OperationNode)result.Value;
            operationNode.OperationType.Should().Be(OperationTypes.MULTIPLY);
            operationNode.FirstOperant.Should().BeOfType<NumberValueNode>();
            operationNode.SecondOperant.Should().BeOfType<NumberValueNode>();
            var firstOperantNode = (NumberValueNode)operationNode.FirstOperant;
            var secondOperantNode = (NumberValueNode)operationNode.SecondOperant;
            firstOperantNode.Value.Should().Be(1);
            secondOperantNode.Value.Should().Be(7);
        }

        // Test für 2 Operations ohne Punkt vor Strich
        [Fact]
        public void CreateByTokens_ShouldConstructExpressionNode_GivenTwoOperationsWithoutMultiBeforeAdd()
        {
            //2 * 3 - 6
            var input = new List<Token>() { tf.Num(2), tf.Mul(), tf.Num(3), tf.Sub(), tf.Num(6) };
            var result = ExpressionNode.CreateByTokens(input);

            result.Value.Should().BeOfType<OperationNode>();
            OperationNode operationNode = (OperationNode)result.Value;

            operationNode.OperationType.Should().Be(OperationTypes.MINUS);
            operationNode.FirstOperant.Should().BeOfType<OperationNode>();
            operationNode.SecondOperant.Should().BeOfType<NumberValueNode>();

            var firstOperant = (OperationNode)operationNode.FirstOperant;
            firstOperant.OperationType.Should().Be(OperationTypes.MULTIPLY);
            firstOperant.FirstOperant.Should().BeOfType<NumberValueNode>();
            firstOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
        }

        // Test für 2 Operations mit Punkt vor Strich
        [Fact]
        public void CreateByTokens_ShouldConstructExpressionNode_GivenTwoOperationsWithMultiBeforeAdd()
        {
            //6 - 2 * 3
            var input = new List<Token>() {tf.Num(6), tf.Sub(), tf.Num(2), tf.Mul(), tf.Num(3)};
            var result = ExpressionNode.CreateByTokens(input);

            result.Value.Should().BeOfType<OperationNode>();
            OperationNode operationNode = (OperationNode)result.Value;

            operationNode.OperationType.Should().Be(OperationTypes.MINUS);
            operationNode.FirstOperant.Should().BeOfType<NumberValueNode>();
            operationNode.SecondOperant.Should().BeOfType<OperationNode>();

            var secondOperant = (OperationNode)operationNode.SecondOperant;
            secondOperant.OperationType.Should().Be(OperationTypes.MULTIPLY);
            secondOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            secondOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
        }
        // Test für 2 Operations mit Klammern
        // Test mit **

        // Tests für invalide Ausdrücke

        

    }

}

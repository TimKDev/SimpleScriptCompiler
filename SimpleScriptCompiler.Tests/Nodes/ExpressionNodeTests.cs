using FluentAssertions;
using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;
using TF = TokenFactory;

namespace SimpleScriptCompiler.Tests.Nodes
{
    public class ExpressionNodeTests
    {
        [Fact]
        public void CreateByTokens_ShouldConstructExpression_GivenSimpleAddition()
        {
            //1 + test
            var input = new List<Token>() { TF.Num(1), TF.Add(), TF.Var("test") };
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
            var input = new List<Token>() { TF.Num(1), TF.Mul(), TF.Num(7) };
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
            var input = new List<Token>() { TF.Num(2), TF.Mul(), TF.Num(3), TF.Sub(), TF.Num(6) };
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
            var input = new List<Token>() { TF.Num(6), TF.Sub(), TF.Num(2), TF.Mul(), TF.Num(3) };
            var result = ExpressionNode.CreateByTokens(input);

            result.Value.Should().BeOfType<OperationNode>();
            OperationNode operationNode = (OperationNode)result.Value;

            operationNode.OperationType.Should().Be(OperationTypes.MINUS);
            operationNode.FirstOperant.Should().BeOfType<NumberValueNode>();
            operationNode.SecondOperant.Should().BeOfType<OperationNode>();

            var secondOperant = (OperationNode)operationNode.SecondOperant;
            secondOperant.OperationType.Should().Be(OperationTypes.MULTIPLY);
            secondOperant.FirstOperant.Should().BeOfType<NumberValueNode>();
            secondOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
        }

        [Fact]
        public void CreateByTokens_ShouldConstructExpressionNode_GivenThreeOperationsWithMultiBeforeAdd()
        {
            //3 * 4 + 5 * 6 
            var input = new List<Token>() { TF.Num(3), TF.Mul(), TF.Num(4), TF.Add(), TF.Num(5), TF.Mul(), TF.Num(6) };
            var result = ExpressionNode.CreateByTokens(input);

            result.Value.Should().BeOfType<OperationNode>();
            OperationNode operationNode = (OperationNode)result.Value;

            operationNode.OperationType.Should().Be(OperationTypes.PLUS);
            operationNode.FirstOperant.Should().BeOfType<OperationNode>();
            operationNode.SecondOperant.Should().BeOfType<OperationNode>();

            var firstOperant = (OperationNode)operationNode.FirstOperant;
            firstOperant.OperationType.Should().Be(OperationTypes.MULTIPLY);
            firstOperant.FirstOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)firstOperant.FirstOperant).Value.Should().Be(3);
            firstOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)firstOperant.SecondOperant).Value.Should().Be(4);


            var secondOperant = (OperationNode)operationNode.SecondOperant;
            secondOperant.OperationType.Should().Be(OperationTypes.MULTIPLY);
            secondOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)secondOperant.FirstOperant).Value.Should().Be(5);
            secondOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)secondOperant.SecondOperant).Value.Should().Be(6);
        }

        // Test für 2 Operations mit Klammern
        [Fact]
        public void CreateByTokens_ShouldConstructExpressionNode_GivenBrackets()
        {
            // (2 + 3) * (4 + 5)
            var input = new List<Token>() { TF.Open(), TF.Num(2), TF.Add(), TF.Num(3), TF.Close(), TF.Mul(), TF.Open(), TF.Num(4), TF.Add(), TF.Num(5), TF.Close() };
            var result = ExpressionNode.CreateByTokens(input);

            result.Value.Should().BeOfType<OperationNode>();
            OperationNode operationNode = (OperationNode)result.Value;

            operationNode.OperationType.Should().Be(OperationTypes.MULTIPLY);
            operationNode.FirstOperant.Should().BeOfType<OperationNode>();
            operationNode.SecondOperant.Should().BeOfType<OperationNode>();

            var firstOperant = (OperationNode)operationNode.FirstOperant;
            firstOperant.OperationType.Should().Be(OperationTypes.PLUS);
            firstOperant.FirstOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)firstOperant.FirstOperant).Value.Should().Be(2);
            firstOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)firstOperant.SecondOperant).Value.Should().Be(3);


            var secondOperant = (OperationNode)operationNode.SecondOperant;
            secondOperant.OperationType.Should().Be(OperationTypes.PLUS);
            secondOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)secondOperant.FirstOperant).Value.Should().Be(4);
            secondOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)secondOperant.SecondOperant).Value.Should().Be(5);
        }

        [Fact]
        public void CreateByTokens_ShouldConstructExpressionNode_GivenMultipleBrackets()
        {
            // ((2 + (3)) * ((4 + 5)))
            var input = new List<Token>() { TF.Open(), TF.Open(), TF.Num(2), TF.Add(), TF.Open(), TF.Num(3), TF.Close(), TF.Close(), TF.Mul(), TF.Open(), TF.Open(), TF.Num(4), TF.Add(), TF.Num(5), TF.Close(), TF.Close(), TF.Close() };
            var result = ExpressionNode.CreateByTokens(input);

            result.Value.Should().BeOfType<OperationNode>();
            OperationNode operationNode = (OperationNode)result.Value;

            operationNode.OperationType.Should().Be(OperationTypes.MULTIPLY);
            operationNode.FirstOperant.Should().BeOfType<OperationNode>();
            operationNode.SecondOperant.Should().BeOfType<OperationNode>();

            var firstOperant = (OperationNode)operationNode.FirstOperant;
            firstOperant.OperationType.Should().Be(OperationTypes.PLUS);
            firstOperant.FirstOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)firstOperant.FirstOperant).Value.Should().Be(2);
            firstOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)firstOperant.SecondOperant).Value.Should().Be(3);

            var secondOperant = (OperationNode)operationNode.SecondOperant;
            secondOperant.OperationType.Should().Be(OperationTypes.PLUS);
            secondOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)secondOperant.FirstOperant).Value.Should().Be(4);
            secondOperant.SecondOperant.Should().BeOfType<NumberValueNode>();
            ((NumberValueNode)secondOperant.SecondOperant).Value.Should().Be(5);
        }

         [Fact]
        public void CreateByTokens_ShouldConstructExpressionNode_GivenOneNumber()
        {
            // 42
            var input = new List<Token>() { TF.Num(42)};
            var result = ExpressionNode.CreateByTokens(input);

            result.Value.Should().BeOfType<NumberValueNode>();
            NumberValueNode numberNode = (NumberValueNode)result.Value;
            numberNode.Value.Should().Be(42);
        }

        [Fact]
        public void CreateByTokens_ShouldConstructExpressionNode_GivenOneNumberWithBrackets()
        {
            // (((42)))
            var input = new List<Token>() { TF.Open(), TF.Open(), TF.Open(), TF.Num(42), TF.Close(), TF.Close(), TF.Close()};
            var result = ExpressionNode.CreateByTokens(input);

            result.Value.Should().BeOfType<NumberValueNode>();
            NumberValueNode numberNode = (NumberValueNode)result.Value;
            numberNode.Value.Should().Be(42);
        }

        // Test mit **

        // Test mit negativen Ausdrücken, z.B. (-3)*5

        // Tests mit < >, <= und >=

        // Tests für Boolian Expressions: true || (false && isAdmin)

        // Tests für invalide Ausdrücke



    }

}

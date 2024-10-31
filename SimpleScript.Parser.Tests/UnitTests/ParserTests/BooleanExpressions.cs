using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests;

public class BooleanExpressions
{
    private readonly IParser _sut = ParserFactory.Create();

    [Fact]
    public void BooleanAssignmentToTrue()
    {
        //LET isCool = TRUE 
        List<Token> programTokens = [TF.Let(), TF.Var("isCool"), TF.Assign(), TF.True()];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
        programNode.AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<BooleanNode>("isCool")
            .AssertBoolean(true);
    }

    [Fact]
    public void BooleanAssignmentToFalse()
    {
        //LET isDev = FALSE
        List<Token> programTokens = [TF.Let(), TF.Var("isCool"), TF.Assign(), TF.False()];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
        programNode.AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<BooleanNode>("isCool")
            .AssertBoolean(false);
    }

    [Fact]
    public void BooleanAssignmentToNumberEqualityExpression()
    {
        //LET areNumbersEqual = x == 7 * 3
        List<Token> programTokens =
            [TF.Let(), TF.Var("areNumbersEqual"), TF.Assign(), TF.Var("x"), TF.Equal(), TF.Num(7), TF.Mul(), TF.Num(3)];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));

        (VariableNode variableNode, MultiplyNode multiplyNode) = programNode
            .AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<EqulityNode>("areNumbersEqual")
            .AssertEquality<VariableNode, MultiplyNode>();

        variableNode.AssertVariable("x");
        (NumberNode firstNumber, NumberNode secondNumber) = multiplyNode.AssertMultiplication<NumberNode, NumberNode>();
        firstNumber.AssertNumber(7);
        secondNumber.AssertNumber(3);
    }

    [Fact]
    public void BooleanAssignmentToNumberInEqualityExpression()
    {
        //LET areNumbersEqual = x != 7 * 3
        List<Token> programTokens =
        [
            TF.Let(), TF.Var("areNumbersEqual"), TF.Assign(), TF.Var("x"), TF.NotEqual(), TF.Num(7), TF.Mul(), TF.Num(3)
        ];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));

        (VariableNode variableNode, MultiplyNode multiplyNode) = programNode
            .AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<InEqulityNode>("isCool")
            .AssertInEquality<VariableNode, MultiplyNode>();

        variableNode.AssertVariable("x");
        (NumberNode firstNumber, NumberNode secondNumber) = multiplyNode.AssertMultiplication<NumberNode, NumberNode>();
        firstNumber.AssertNumber(7);
        secondNumber.AssertNumber(3);
    }

    [Fact]
    public void BooleanAssignmentToNumberInSmallerExpression()
    {
        //LET areNumbersEqual = x < 7 * 3
        List<Token> programTokens =
        [
            TF.Let(), TF.Var("areNumbersEqual"), TF.Assign(), TF.Var("x"), TF.Smaller(), TF.Num(7), TF.Mul(), TF.Num(3)
        ];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));

        (VariableNode variableNode, MultiplyNode multiplyNode) = programNode
            .AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<SmallerNode>("isCool")
            .AssertSmaller<VariableNode, MultiplyNode>();

        variableNode.AssertVariable("x");
        (NumberNode firstNumber, NumberNode secondNumber) = multiplyNode.AssertMultiplication<NumberNode, NumberNode>();
        firstNumber.AssertNumber(7);
        secondNumber.AssertNumber(3);
    }

    [Fact]
    public void BooleanAssignmentToNumberInGreaterExpression()
    {
        //LET areNumbersEqual = x > 7 * 3
        List<Token> programTokens =
        [
            TF.Let(), TF.Var("areNumbersEqual"), TF.Assign(), TF.Var("x"), TF.Greater(), TF.Num(7), TF.Mul(), TF.Num(3)
        ];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));

        (VariableNode variableNode, MultiplyNode multiplyNode) = programNode
            .AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<GreaterNode>("isCool")
            .AssertGreater<VariableNode, MultiplyNode>();

        variableNode.AssertVariable("x");
        (NumberNode firstNumber, NumberNode secondNumber) = multiplyNode.AssertMultiplication<NumberNode, NumberNode>();
        firstNumber.AssertNumber(7);
        secondNumber.AssertNumber(3);
    }

    [Fact]
    public void BooleanAssignmentToNumberInGreaterOrEqualExpression()
    {
        //LET areNumbersEqual = x >= 7 * 3
        List<Token> programTokens =
        [
            TF.Let(), TF.Var("areNumbersEqual"), TF.Assign(), TF.Var("x"), TF.GreaterOrEqual(), TF.Num(7), TF.Mul(),
            TF.Num(3)
        ];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));

        (VariableNode variableNode, MultiplyNode multiplyNode) = programNode
            .AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<GreaterOrEqualNode>("isCool")
            .AssertGreaterOrEqual<VariableNode, MultiplyNode>();

        variableNode.AssertVariable("x");
        (NumberNode firstNumber, NumberNode secondNumber) = multiplyNode.AssertMultiplication<NumberNode, NumberNode>();
        firstNumber.AssertNumber(7);
        secondNumber.AssertNumber(3);
    }

    [Fact]
    public void BooleanAssignmentToNumberInSmallerOrEqualExpression()
    {
        //LET areNumbersEqual = x <= 7 * 3
        List<Token> programTokens =
        [
            TF.Let(), TF.Var("areNumbersEqual"), TF.Assign(), TF.Var("x"), TF.SmallerOrEqual(), TF.Num(7), TF.Mul(),
            TF.Num(3)
        ];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));

        (VariableNode variableNode, MultiplyNode multiplyNode) = programNode
            .AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<SmallerOrEqualNode>("isCool")
            .AssertSmallerOrEqual<VariableNode, MultiplyNode>();

        variableNode.AssertVariable("x");
        (NumberNode firstNumber, NumberNode secondNumber) = multiplyNode.AssertMultiplication<NumberNode, NumberNode>();
        firstNumber.AssertNumber(7);
        secondNumber.AssertNumber(3);
    }

    [Fact]
    public void BooleanAssignmentToStringEqualityExpression()
    {
        //LET areStringsEqual = name == "Caro"
        List<Token> programTokens =
            [TF.Let(), TF.Var("areStringEqual"), TF.Assign(), TF.Var("name"), TF.Equal(), TF.Str("Caro")];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));

        (VariableNode variableNode, StringNode stringNode) = programNode
            .AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<EqulityNode>("areStringEqual")
            .AssertEquality<VariableNode, StringNode>();

        variableNode.AssertVariable("name");
        stringNode.AssertString("Caro");
    }

    [Fact]
    public void BooleanAssignmentToStringInEqualityExpression()
    {
        //LET areNotStringsEqual = name != "Caro"
        List<Token> programTokens =
            [TF.Let(), TF.Var("areNotStringEqual"), TF.Assign(), TF.Var("name"), TF.Equal(), TF.Str("Caro")];
        ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));

        (VariableNode variableNode, StringNode stringNode) = programNode
            .AssertProgramNode<VariableDeclarationNode>()
            .AssertVariableDeclarationWithInit<InEqulityNode>("areStringEqual")
            .AssertInEquality<VariableNode, StringNode>();

        variableNode.AssertVariable("name");
        stringNode.AssertString("Caro");
    }
}
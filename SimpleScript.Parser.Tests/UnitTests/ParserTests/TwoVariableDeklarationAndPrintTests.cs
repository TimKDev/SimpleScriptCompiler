﻿using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class TwoVariableDeklarationAndPrintTests
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ShouldDeklareTwoVariables_And_AddThemInPrint()
        {
            List<Token> programTokens = [TF.Let(), TF.Var("num1"), TF.Assign(), TF.Num(1), TF.Let(), TF.Var("num2"), TF.Assign(), TF.Num(2), TF.Print(), TF.Var("num1"), TF.Add(), TF.Var("num2")];
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            (VariableDeclarationNode firstDeklaration, VariableDeclarationNode secondDeklaration, PrintNode printNode) = programNode.Assert<VariableDeclarationNode, VariableDeclarationNode, PrintNode>();
            firstDeklaration.AssertVariableDeclarationWithInit<NumberNode>("num1").AssertNumber(1);
            secondDeklaration.AssertVariableDeclarationWithInit<NumberNode>("num2").AssertNumber(2);
            (VariableNode var1, VariableNode var2) = printNode.AssertPrint<AddNode>().AssertAddition<VariableNode, VariableNode>();
            var1.AssertVariable("num1");
            var2.AssertVariable("num2");
        }
    }
}

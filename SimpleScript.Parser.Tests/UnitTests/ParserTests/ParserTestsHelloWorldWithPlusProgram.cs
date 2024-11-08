﻿using FluentAssertions;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
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
            PrintNode printNode = programmingNode.AssertProgramNode<PrintNode>();
            NH.AssertPrintNode<AddNode>(printNode);
        }

        [Fact]
        public void ParserTests_AddNodeShouldHaveTwoStringsAsChildren()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = programmingNode.AssertProgramNode<PrintNode>();
            AddNode addNode = NH.AssertPrintNode<AddNode>(printNode);
            NH.AssertAddNode<StringNode, StringNode>(addNode);
        }

        [Fact]
        public void ParserTests_StringsShouldHaveValuesHelloAndWorld()
        {
            ProgramNode programmingNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(ProgramTokens));
            PrintNode printNode = programmingNode.AssertProgramNode<PrintNode>();
            AddNode addNode = NH.AssertPrintNode<AddNode>(printNode);
            (StringNode firstString, StringNode secondString) = NH.AssertAddNode<StringNode, StringNode>(addNode);
            firstString.Value.Should().Be(Hello);
            secondString.Value.Should().Be(World);
        }
    }
}

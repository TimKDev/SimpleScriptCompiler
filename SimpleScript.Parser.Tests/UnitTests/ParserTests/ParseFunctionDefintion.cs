using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;
using Xunit.Abstractions;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParseFunctionDefintion(ITestOutputHelper testOutputHelper)
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParseTokens_ShouldAddFunctionNodeToProgram_GivenTokens()
        {
            //FUNC add(int num_1, int num_2)
            //BODY
            //    LET result = num_1 + num_2
            //    RETURN result
            //ENDBODY
            List<Token> programTokens =
            [
                TF.Func(), TF.Var("add"), TF.Open(), TF.Int(), TF.Var("num_1"), TF.Comma(), TF.Int(), TF.Var("num_2"),
                TF.Close(), TF.Body(), TF.Let(), TF.Var("result"), TF.Assign(), TF.Var("num_1"), TF.Add(),
                TF.Var("num_2"), TF.Return(), TF.Var("result"), TF.EndBody()
            ];

            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            programNode.AssertProgramNode<FunctionNode>();
        }

        [Fact]
        public void ParseTokens_ShouldAddTwoIntArgumentDefinitions_GivenTokens()
        {
            List<Token> programTokens =
            [
                TF.Func(), TF.Var("add"), TF.Open(), TF.Int(), TF.Var("num_1"), TF.Comma(), TF.Int(), TF.Var("num_2"),
                TF.Close(), TF.Body(), TF.Let(), TF.Var("result"), TF.Assign(), TF.Var("num_1"), TF.Add(),
                TF.Var("num_2"), TF.Return(), TF.Var("result"), TF.EndBody()
            ];

            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            programNode
                .AssertProgramNode<FunctionNode>()
                .AssertFunctionNode("add", [(ArgumentType.Int, "num_1"), (ArgumentType.Int, "num_2")]);
        }

        [Fact]
        public void ParseTokens_ShouldAddOneStringArgumentDefinitions_GivenTokens()
        {
            List<Token> programTokens =
            [
                TF.Func(), TF.Var("returnString"), TF.Open(), TF.StringArg(), TF.Var("myString"), TF.Close(), TF.Body(),
                TF.Return(), TF.Var("myString"), TF.EndBody()
            ];

            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            programNode
                .AssertProgramNode<FunctionNode>()
                .AssertFunctionNode("returnString", [(ArgumentType.String, "myString")]);
        }

        [Fact]
        public void ParseTokens_ShouldAddOneBooleanArgumentDefinitions_GivenTokens()
        {
            List<Token> programTokens =
            [
                TF.Func(), TF.Var("returnBoolean"), TF.Open(), TF.BooleanArg(), TF.Var("myBool"), TF.Close(), TF.Body(),
                TF.Return(), TF.Var("myBool"), TF.EndBody()
            ];

            ProgramNode programNode =
                ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens), testOutputHelper);
            programNode
                .AssertProgramNode<FunctionNode>()
                .AssertFunctionNode("returnBoolean", [(ArgumentType.Boolean, "myBool")]);
        }

        [Fact]
        public void ParseTokens_ShouldCreateBody_GivenTokens()
        {
            List<Token> programTokens =
            [
                TF.Func(), TF.Var("add"), TF.Open(), TF.Int(), TF.Var("num_1"), TF.Comma(), TF.Int(), TF.Var("num_2"),
                TF.Close(), TF.Body(), TF.Let(), TF.Var("result"), TF.Assign(), TF.Var("num_1"), TF.Add(),
                TF.Var("num_2"), TF.Return(), TF.Var("result"), TF.EndBody()
            ];
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));

            programNode
                .AssertProgramNode<FunctionNode>()
                .AssertFunctionNode("add", [(ArgumentType.Int, "num_1"), (ArgumentType.Int, "num_2")])
                .AssertBody<VariableDeclarationNode, ReturnNode>();
        }
    }
}
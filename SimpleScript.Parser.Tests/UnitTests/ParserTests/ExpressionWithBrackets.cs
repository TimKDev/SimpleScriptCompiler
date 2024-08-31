using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Extensions;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ExpressionWithBrackets
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void TwoMultiplicationsThatAreAdded()
        {
            List<Token> programTokens = [TF.Print(), TF.Num(1), TF.Mul(), TF.Num(6), TF.Add(), TF.Num(2), TF.Mul(), TF.Num(7)];
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            (MultiplyNode? multiplyNode1, MultiplyNode? multiplyNode2) = programNode
                .AssertProgramNode<PrintNode>()
                .Assert<AddNode>()
                .Assert<MultiplyNode, MultiplyNode>();

            (NumberNode addNodeNumber1, NumberNode addNodeNumber2) = multiplyNode1.Assert<NumberNode, NumberNode>();
            addNodeNumber1.Assert(1);
            addNodeNumber2.Assert(6);

            (NumberNode addNodeNumber3, NumberNode addNodeNumber4) = multiplyNode2.Assert<NumberNode, NumberNode>();
            addNodeNumber3.Assert(2);
            addNodeNumber4.Assert(7);
        }

        [Fact]
        public void ExpressionWithDistLaw()
        {
            List<Token> programTokens = [TF.Print(), TF.Open(), TF.Num(1), TF.Add(), TF.Num(6), TF.Close(), TF.Mul(), TF.Num(2)];
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            (AddNode? addNode, NumberNode? num2) = programNode
                .AssertProgramNode<PrintNode>()
                .Assert<MultiplyNode>()
                .Assert<AddNode, NumberNode>();

            (NumberNode addNodeNumber1, NumberNode addNodeNumber2) = addNode.Assert<NumberNode, NumberNode>();
            addNodeNumber1.Assert(1);
            addNodeNumber2.Assert(6);
            num2.Assert(2);
        }

        [Fact]
        public void ExpressionWithFunctionInsideABracket()
        {

            List<Token> programTokens = [TF.Print(), TF.Open(), TF.Var("test"), TF.Open(), TF.Close(), TF.Add(), TF.Num(5), TF.Close(), TF.Mul(), TF.Num(3)];
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            (AddNode? addNode, NumberNode? numberNode) = programNode
                .AssertProgramNode<PrintNode>()
                .Assert<MultiplyNode>()
                .Assert<AddNode, NumberNode>();
            numberNode.Assert(3);
            (FunctionInvocationNode functionInvokation, NumberNode numberNodeInsideBracket) = addNode.Assert<FunctionInvocationNode, NumberNode>();
            numberNodeInsideBracket.Assert(5);
            functionInvokation.Assert("test");
        }

        [Fact]
        public void ExpressionWithRedundantBrackets()
        {

            List<Token> programTokens = [TF.Print(), TF.Open(), TF.Open(), TF.Num(1), TF.Add(), TF.Num(1), TF.Close(), TF.Close(), TF.Mul(), TF.Num(5)];
            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            (AddNode? addNode, NumberNode? numberNode) = programNode
                .AssertProgramNode<PrintNode>()
                .Assert<MultiplyNode>()
                .Assert<AddNode, NumberNode>();
            numberNode.Assert(5);
            (NumberNode numberNodeInAdd1, NumberNode numberNodeInAdd2) = addNode.Assert<NumberNode, NumberNode>();
            numberNodeInAdd1.Assert(1);
            numberNodeInAdd2.Assert(1);
        }

        [Fact]
        public void ErrorShouldBeThrownWhenNumberOfOpeningAndClosedBracketsDiffer()
        {

            List<Token> programTokens = [TF.Print(), TF.Open(), TF.Open(), TF.Open(), TF.Num(3), TF.Add(), TF.Num(3), TF.Close(), TF.Close()];
            ErrorHelper.AssertErrors(_sut.ParseTokens(programTokens), [ErrorHelper.CreateErrorMessage("Number of Brackets are not equal", 1)]);
        }
    }
}

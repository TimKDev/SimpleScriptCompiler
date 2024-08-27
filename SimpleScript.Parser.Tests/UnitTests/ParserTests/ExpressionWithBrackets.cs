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
        //Test with two Backets which are multiplied: (2 + 5) * (4 + 9)

        //Test with Function inside a Bracket of add operation: (test() + 5) * 3

        //Test with redundant Brackets: ((1 + 1)) * 5

        //Test that an error is thrown when a Number of opening brackets does not match Number of closing Brackets: (((3 + 3)) 

        //Test that an error is thrown when a empty bracket is not a function call for example 2() + 5
    }
}

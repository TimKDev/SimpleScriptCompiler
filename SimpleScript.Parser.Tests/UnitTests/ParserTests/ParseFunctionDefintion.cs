using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Tests.Helper.Factories;
using SimpleScript.Tests.Shared;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests
{
    public class ParseFunctionDefintion
    {
        private readonly Parser _sut = ParserFactory.Create();

        [Fact]
        public void ParseTokens_ShouldCreateInputNodeWithVariableName_GivenInput()
        {
            //FUNC add(int num_1, int num_2)
            //BODY
            //    LET result = num_1 + num_2
            //    RETURN result
            //ENDBODY
            List<Token> programTokens = [TF.Func(), TF.Var("add"), TF.Open(), TF.Int(), TF.Var("num_1"), TF.Comma(), TF.Int(), TF.Var("num_2"), TF.Close(), TF.Body(), TF.Let(), TF.Var("result"), TF.Assign(), TF.Var("num_1"), TF.Add(), TF.Var("num_2"), TF.Return(), TF.Var("result"), TF.EndBody()];

            ProgramNode programNode = ErrorHelper.AssertResultSuccess(_sut.ParseTokens(programTokens));
            FunctionNode functionNode = NH.AssertProgramNode<FunctionNode>(programNode);
        }
    }
}

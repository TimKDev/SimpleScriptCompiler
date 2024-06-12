using SimpleScript.Parser.Tests.Helper;
using SimpleScriptCompiler.LexicalAnalysis;
using TF = SimpleScript.Parser.Tests.Helper.TokenFactory;

namespace SimpleScript.Parser.Tests.ExpressionBuilderTests
{
    public class CreateExpression
    {
        [Fact]
        public void ShouldCreateMulNodeWithTwoNumbers_GivenMultiplication()
        {
            List<Token> inputTokens = [TF.Num(2), TF.Mul(), TF.Num(5)];
            Nodes.AddNode result = ErrorHelper.AssertResultSuccess(ExpressionBuilder.CreateExpression(inputTokens));


        }
    }
}

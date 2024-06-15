using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories;
using SimpleScript.Parser.Nodes;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser
{
    public class Parser
    {
        public Result<ProgramNode> ParseTokens(List<Token> inputTokens)
        {
            ProgramNode programNode = new();
            PrintNode printNode = new();
            if (inputTokens.Select(token => token.TokenType).Contains(TokenType.PLUS))
            {
                List<Token> tokensOfExpression = inputTokens.Skip(1).ToList();
                Result<IExpression> addNodeResult = ExpressionFactory.Create(tokensOfExpression);

                if (!addNodeResult.IsSuccess)
                {
                    return addNodeResult.Convert<ProgramNode>();
                }

                printNode.ChildNodes.Add(addNodeResult.Value);
                programNode.ChildNodes.Add(printNode);
            }
            else
            {
                printNode.ChildNodes.Add(new StringNode(inputTokens[1].Value!));
                programNode.ChildNodes.Add(printNode);
            }

            return programNode;
        }


    }
}

using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser
{
    public class Parser
    {
        private IExpressionFactory _expressionFactory;

        public Parser(IExpressionFactory expressionFactory)
        {
            _expressionFactory = expressionFactory;
        }

        public Result<ProgramNode> ParseTokens(List<Token> inputTokens)
        {
            ProgramNode programNode = new();
            PrintNode printNode = new();
            if (inputTokens.Select(token => token.TokenType).Contains(TokenType.PLUS))
            {
                List<Token> tokensOfExpression = inputTokens.Skip(1).ToList();
                Result<IExpression> addNodeResult = _expressionFactory.Create(tokensOfExpression);

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

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
            if (inputTokens[0].TokenType == TokenType.LET)
            {
                string? variableName = inputTokens[1].Value;
                if (variableName == null)
                {
                    //TTODO Error Handeling
                    throw new ArgumentException();
                }
                VariableDeclarationNode variableDeklarationNode = new(variableName);
                programNode.ChildNodes.Add(variableDeklarationNode);
            }
            else if (inputTokens[0].TokenType == TokenType.PRINT)
            {
                PrintNode printNode = new();
                programNode.ChildNodes.Add(printNode);
                List<Token> tokensOfExpression = inputTokens.Skip(1).ToList();
                Result<IExpression> printExpression = _expressionFactory.Create(tokensOfExpression);
                //TTODO Error Handeling
                printNode.ChildNodes.Add(printExpression.Value);
            }

            return programNode;
        }


    }
}

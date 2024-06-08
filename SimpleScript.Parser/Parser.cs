using EntertainingErrors;
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
                int positionOfPlus = inputTokens.FindIndex(token => token.TokenType == TokenType.PLUS);
                AddNode addNode = new();
                if (positionOfPlus == 0)
                {
                    return Error.Create("Plus Operation is missing first operant.");
                }
                string? firstString = inputTokens[positionOfPlus - 1].Value;
                string? secondString = inputTokens[positionOfPlus + 1].Value;
                addNode.ChildNodes.Add(new StringNode(firstString));
                addNode.ChildNodes.Add(new StringNode(secondString));
                printNode.ChildNodes.Add(addNode);
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

using EntertainingErrors;
using SimpleScript.Parser.Nodes;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser
{
    public static class ExpressionBuilder
    {
        public static Result<AddNode> CreateExpression(List<Token> inputTokens)
        {
            return CreateAddNode(inputTokens);
        }

        private static Result<AddNode> CreateAddNode(List<Token> inputTokens)
        {
            AddNode addNode = new();
            int positionOfPlus = inputTokens.FindIndex(token => token.TokenType == TokenType.PLUS);
            if (positionOfPlus == 0)
            {
                return Error.Create("Plus Operation is missing first operant.");
            }
            Token firstOperant = inputTokens[positionOfPlus - 1];
            Token secondOperant = inputTokens[positionOfPlus + 1];
            IAddable? firstValue;
            IAddable? secondValue;
            if (firstOperant.TokenType == TokenType.String && secondOperant.TokenType == TokenType.String)
            {
                firstValue = new StringNode(firstOperant.Value!);
                secondValue = new StringNode(secondOperant.Value!);
            }
            else if (firstOperant.TokenType == TokenType.Number && secondOperant.TokenType == TokenType.Number)
            {
                firstValue = new NumberNode(int.Parse(firstOperant.Value!));
                secondValue = new NumberNode(int.Parse(secondOperant.Value!));
            }
            else
            {
                return Error.Create("Plus Operation is not allowed.");
            }
            addNode.ChildNodes.Add(firstValue);
            addNode.ChildNodes.Add(secondValue);

            return addNode;
        }
    }
}

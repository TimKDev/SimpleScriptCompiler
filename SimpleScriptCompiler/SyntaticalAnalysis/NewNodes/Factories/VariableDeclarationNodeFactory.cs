namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Factories
{


    //public class VariableDeclarationNodeFactory : INodeFactory
    //{
    //    public Result<int> AddNodeToParent(IHasChildNodes node, Token[] tokens, int startPosition)
    //    {
    //        if (startPosition + 1 > tokens.Length && tokens[startPosition].TokenType != TokenType.LET || tokens[startPosition + 1].TokenType != TokenType.Variable)
    //        {
    //            return Error.Create("Rule: Es müssen 1 oder 3 Tokens folgen mit Identifier(, ASSERT und Initvalue)");
    //        }

    //        int nextStartPosition = startPosition + 2;

    //        string name = tokens[startPosition + 1].Value!;
    //        int start = tokens[startPosition].Line;
    //        int end = tokens[startPosition + 1].Line;

    //        VariableDeclarationNode variableDeklarationNode = new(name, start, end);

    //        if (startPosition + 3 > tokens.Length && tokens[startPosition + 2].TokenType == TokenType.ASSIGN)
    //        {

    //            ExpressionNodeFactory expressionNodeFactory = new();
    //            Result<int> expressionCreationResult = expressionNodeFactory.AddNodeToParent(variableDeklarationNode, tokens, startPosition + 2);
    //            if (expressionCreationResult.IsSuccess)
    //            {
    //                nextStartPosition = expressionCreationResult.Value;
    //            }
    //        }

    //        node.AddChildNode(variableDeklarationNode);

    //        return nextStartPosition;
    //    }
    //}
}

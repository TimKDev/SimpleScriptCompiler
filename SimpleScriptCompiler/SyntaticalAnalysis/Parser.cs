using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes;

namespace SimpleScriptCompiler.SyntaticalAnalysis
{
  //Aufgabe des Parsers ist es herauszufinden welche Tokens zu einer Node oder ihren Parent Nodes gehören. Die Nodes sind dann dafür zuständig mit Hilfe von Factory Methoden aus den Tokens die entsprechende Node zu erstellen.
  public class Parser
    {

        //LET a = 0
        public ProgramNode ParseToAST(List<Token> tokens)
        {
            var program = new ProgramNode();
            var i = 0;
            while (i < tokens.Count)
            {
                var token = tokens[i];
                if (token.TokenType == TokenType.LET)
                {
                    var variableDeklarationTokens = GetVariableDeklarationTokensFromStartPosition(tokens, i);
                    var variableDeklarationToken = VariableDeklarationNode.CreateFromTokens(variableDeklarationTokens);
                    program.ChildNodes.Add(variableDeklarationToken);
                    i += variableDeklarationTokens.Count;
                    continue;
                }
                i++;
            }

            return program;
        }

        private List<Token> GetVariableDeklarationTokensFromStartPosition(List<Token> tokens, int startPosition)
        {
            if (startPosition + 1 > tokens.Count)
            {
                throw new Exception("Invalid Syntax"); //Rule: Es müssen 1 oder 3 Tokens folgen mit Identifier(, ASSERT und Initvalue)
            }

            var variableDeklarationTokens = new List<Token>() {
                tokens[startPosition],
                tokens[startPosition + 1]
            };

            if (startPosition + 3 < tokens.Count && tokens[startPosition + 2].TokenType == TokenType.ASSIGN)
            {
                //Beispiel: LET test = a + b * 2
                variableDeklarationTokens.Add(tokens[startPosition + 2]);
                var expressionTokens = GetExpressionFromStartPosition(tokens, startPosition + 3);
                variableDeklarationTokens.AddRange(expressionTokens);
                return variableDeklarationTokens;
            }

            //Beispiel: LET test
            return variableDeklarationTokens;
        }

        private List<Token> GetExpressionFromStartPosition(List<Token> tokens, int startPosition)
        {
            List<Token> expression = [];
            for (int i = startPosition; i < tokens.Count; i++)
            {
                if (!ExpressionNode.SupportedTokenTypes.Contains(tokens[i].TokenType))
                {
                    return expression;
                }
                expression.Add(tokens[i]);
            }
            return expression;
        }
    }
}

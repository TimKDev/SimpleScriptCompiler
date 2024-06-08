using EntertainingErrors;
using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Nodes;

namespace SimpleScriptCompiler.SyntaticalAnalysis.NewNodes.Factories
{
    public class ProgramNodeFactory
    {
        public static ProgramNode Create(Token[] tokens)
        {
            int startLine = tokens[0].Line;
            int endLine = tokens[^1].Line;
            ProgramNode program = new(startLine, endLine);
            int i = 0;
            while (i < tokens.Length)
            {
                Token token = tokens[i];
                if (token.TokenType == TokenType.LET)
                {
                    VariableDeclarationNodeFactory variableDeklarationNodeFactory = new();
                    Result<int> variableCreationResult = variableDeklarationNodeFactory.AddNodeToParent(program, tokens, i);
                    if (variableCreationResult.IsSuccess)
                    {
                        i = variableCreationResult.Value;
                    }
                    continue;
                }
                i++;
            }

            return program;
        }
    }
}

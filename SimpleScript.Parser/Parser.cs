using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;
using SimpleScriptCompiler.LexicalAnalysis;

namespace SimpleScript.Parser
{
    public class Parser : IParser
    {
        private readonly IStatementCombiner _statementCombiner;
        private readonly IVariableDeclarartionNodeFactory _variableAssignmentFactory;
        private readonly IPrintNodeFactory _printNodeFactory;

        public Parser(IStatementCombiner statementCombiner, IVariableDeclarartionNodeFactory variableAssignmentFactory, IPrintNodeFactory printNodeFactory)
        {
            _statementCombiner = statementCombiner;
            _variableAssignmentFactory = variableAssignmentFactory;
            _printNodeFactory = printNodeFactory;
        }

        public Result<ProgramNode> ParseTokens(List<Token> inputTokens)
        {
            List<Error> errors = [];
            ProgramNode programNode = new();
            Result<List<Statement>> statementResult = _statementCombiner.CreateStatements(inputTokens);

            if (!statementResult.IsSuccess)
            {
                return statementResult.Errors;
            }

            foreach (Statement statement in statementResult.Value)
            {
                Result<IProgramRootNodes> result = statement.Tokens[0].TokenType switch
                {
                    TokenType.LET => _variableAssignmentFactory.Create(statement.Tokens).Convert<IProgramRootNodes>(),
                    TokenType.PRINT => _printNodeFactory.Create(statement.Tokens).Convert<IProgramRootNodes>(),
                    _ => Error.Create("Compiler Error: Unknown Statement Type.")
                };
                errors.AddRange(result.Errors);
                if (result.IsSuccess)
                {
                    programNode.ChildNodes.Add(result.Value);
                }
            }

            return errors.Count != 0 ? errors : programNode;
        }
    }
}

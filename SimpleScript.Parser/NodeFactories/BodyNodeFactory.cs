using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories
{
    public class BodyNodeFactory : IBodyNodeFactory
    {
        private readonly IStatementCombiner _statementCombiner;
        private readonly IVariableDeclarartionNodeFactory _variableAssignmentFactory;
        private readonly IPrintNodeFactory _printNodeFactory;
        private readonly IInputNodeFactory _inputNodeFactory;
        private readonly IFunctionNodeFactory _functionNodeFactory;
        private readonly IReturnNodeFactory _returnNodeFactory;

        public BodyNodeFactory(IStatementCombiner statementCombiner, IVariableDeclarartionNodeFactory variableAssignmentFactory, IPrintNodeFactory printNodeFactory, IInputNodeFactory inputNodeFactory, IFunctionNodeFactory functionNodeFactory, IReturnNodeFactory returnNodeFactory)
        {
            _statementCombiner = statementCombiner;
            _variableAssignmentFactory = variableAssignmentFactory;
            _printNodeFactory = printNodeFactory;
            _inputNodeFactory = inputNodeFactory;
            _functionNodeFactory = functionNodeFactory;
            _returnNodeFactory = returnNodeFactory;
        }

        public Result<BodyNode> Create(List<Token> inputTokens)
        {
            if (!inputTokens.Any())
            {
                throw new ArgumentException("Empty Body");
            }
            List<Error> errors = [];
            Result<List<Statement>> statementResult = _statementCombiner.CreateStatements(inputTokens);

            if (!statementResult.IsSuccess)
            {
                return statementResult.Errors;
            }

            List<IBodyNode> childNodes = [];
            foreach (Statement statement in statementResult.Value)
            {
                Result<IBodyNode> result = statement.Tokens[0].TokenType switch
                {
                    TokenType.LET => _variableAssignmentFactory.Create(statement.Tokens).Convert<IBodyNode>(),
                    TokenType.PRINT => _printNodeFactory.Create(statement.Tokens).Convert<IBodyNode>(),
                    TokenType.INPUT => _inputNodeFactory.Create(statement.Tokens).Convert<IBodyNode>(),
                    TokenType.FUNC => _functionNodeFactory.Create(statement.Tokens, this).Convert<IBodyNode>(),
                    TokenType.RETURN => _returnNodeFactory.Create(statement.Tokens).Convert<IBodyNode>(),
                    _ => Error.Create("Compiler Error: Unknown Statement Type.")
                };
                errors.AddRange(result.Errors);
                if (result.IsSuccess)
                {
                    childNodes.Add(result.Value);
                }
            }

            int startLine = inputTokens.First().Line;
            int endLine = inputTokens.Last().Line;
            BodyNode bodyNode = new(childNodes, startLine, endLine);

            return errors.Count != 0 ? errors : bodyNode;
        }
    }
}

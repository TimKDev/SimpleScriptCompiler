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
        private IStatementCombiner _statementCombiner;

        public Parser(IExpressionFactory expressionFactory, IStatementCombiner statementCombiner)
        {
            _expressionFactory = expressionFactory;
            _statementCombiner = statementCombiner;
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
            List<Statement> statements = statementResult.Value;
            foreach (Statement statement in statements)
            {
                Token firstTokenOfStatements = statement.Tokens[0];
                if (firstTokenOfStatements.TokenType == TokenType.LET)
                {
                    Result addVariableAssignmentResult = AddVariableAssignmentToProgramNode(programNode, statement.Tokens);
                    if (!addVariableAssignmentResult.IsSuccess)
                    {
                        errors.AddRange(addVariableAssignmentResult.Errors);
                    }
                }
                else if (firstTokenOfStatements.TokenType == TokenType.PRINT)
                {
                    Result addPrintResult = AddPrintToProgramNode(programNode, statement.Tokens);
                    if (!addPrintResult.IsSuccess)
                    {
                        errors.AddRange(addPrintResult.Errors);
                    }
                }
            }

            if (errors.Any())
            {
                return errors;
            }

            return programNode;
        }

        private Result AddVariableAssignmentToProgramNode(ProgramNode programNode, List<Token> inputTokens)
        {
            if (inputTokens.Count < 2 || inputTokens[1].TokenType != TokenType.Variable)
            {
                return inputTokens[0].CreateError("Invalid usage of Let keyword. Let should be followed by a variable name.");
            }
            string? variableName = inputTokens[1].Value;
            if (variableName == null)
            {
                return inputTokens[0].CreateError("Invalid usage of Let keyword. Let should be followed by a variable name not equals to null.");
            }
            VariableDeclarationNode variableDeklarationNode;
            if (inputTokens.Count > 2 && inputTokens[2].TokenType == TokenType.ASSIGN)
            {
                if (inputTokens.Count < 4)
                {
                    return inputTokens[2].CreateError("Missing Assert Value: No value given after the assert symbol.");
                }
                List<Token> tokensOfExpression = inputTokens.Skip(3).ToList();
                Result<IExpression> initialValueExpression = _expressionFactory.Create(tokensOfExpression);
                if (!initialValueExpression.IsSuccess)
                {
                    string errorMessage = $"Invalid Expression: {string.Join(", ", initialValueExpression.Errors.Select(e => e.Message))}";
                    return inputTokens[2].CreateError(errorMessage);
                }
                variableDeklarationNode = new(variableName, initialValueExpression.Value);
            }
            else
            {
                variableDeklarationNode = new(variableName);
            }
            programNode.ChildNodes.Add(variableDeklarationNode);

            return Result.Success();
        }

        private Result AddPrintToProgramNode(ProgramNode programNode, List<Token> inputTokens)
        {

            List<Token> tokensOfExpression = inputTokens.Skip(1).ToList();
            Result<IExpression> printExpression = _expressionFactory.Create(tokensOfExpression);
            //TTODO Error Handeling
            PrintNode printNode = new(printExpression.Value);
            programNode.ChildNodes.Add(printNode);

            return Result.Success();
        }
    }
}

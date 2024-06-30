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
            List<Error> errors = [];
            ProgramNode programNode = new();
            if (inputTokens[0].TokenType == TokenType.LET)
            {
                Result addVariableAssignmentResult = AddVariableAssignmentToProgramNode(programNode, inputTokens);
                if (!addVariableAssignmentResult.IsSuccess)
                {
                    errors.AddRange(addVariableAssignmentResult.Errors);
                }
            }
            else if (inputTokens[0].TokenType == TokenType.PRINT)
            {
                Result addPrintResult = AddPrintToProgramNode(inputTokens, programNode); //TTODO Analog
                if (!addPrintResult.IsSuccess)
                {
                    errors.AddRange(addPrintResult.Errors);
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
                    return initialValueExpression.Convert();
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

        private Result AddPrintToProgramNode(List<Token> inputTokens, ProgramNode programNode)
        {
            PrintNode printNode = new();
            programNode.ChildNodes.Add(printNode);
            List<Token> tokensOfExpression = inputTokens.Skip(1).ToList();
            Result<IExpression> printExpression = _expressionFactory.Create(tokensOfExpression);
            //TTODO Error Handeling
            printNode.ChildNodes.Add(printExpression.Value);

            return Result.Success();
        }
    }
}

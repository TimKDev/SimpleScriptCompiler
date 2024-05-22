using SimpleScriptCompiler.LexicalAnalysis;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Enums;
using SimpleScriptCompiler.SyntaticalAnalysis.Nodes.Interfaces;

namespace SimpleScriptCompiler.SyntaticalAnalysis.Nodes
{
  public class OperationNode : INode, IExpressionPart
  {
    public static TokenType[] SupportedTokenTypes = [
        TokenType.PLUS,
            TokenType.MINUS,
            TokenType.MULTIPLY,
            TokenType.DIVIDE,
            TokenType.POWER,
            TokenType.SMALLER,
            TokenType.GREATER,
            TokenType.SMALLER_OR_EQUAL,
            TokenType.GREATER_OR_EQUAL,
            TokenType.EQUAL,
        ];
    public NodeTypes Type => NodeTypes.Operation;
    public OperationTypes OperationType { get; }
    public IExpressionPart FirstOperant { get; set; }
    public IExpressionPart SecondOperant { get; set; }
    private OperationNode(OperationTypes operationType)
    {
      OperationType = operationType;
    }

    public static OperationNode Create(OperationTypes operationType)
    {
      return new(operationType);
    }

    public static OperationNode Create(OperationTypes operationType, IExpressionPart firstOperant, IExpressionPart secondOperant)
    {
      OperationNode newOperationNode = new(operationType)
      {
        FirstOperant = firstOperant,
        SecondOperant = secondOperant
      };

      return newOperationNode;
    }
  }
}

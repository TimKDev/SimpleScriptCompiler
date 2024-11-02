using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories.Interfaces;

public interface IMinusNodeFactory : IBinaryNodeFactory<MinusNode>
{
    Result<MinusNode> Create(List<Token> firstOperand, List<Token> secondOperand,
        IExpressionFactory expressionFactory);
}
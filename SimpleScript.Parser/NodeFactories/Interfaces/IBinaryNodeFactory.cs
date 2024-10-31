using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;

namespace SimpleScript.Parser.NodeFactories;

public interface IBinaryNodeFactory<TNode>
{
    Result<TNode> Create(List<Token> firstOperand, List<Token> secondOperand,
        IExpressionFactory expressionFactory);
}
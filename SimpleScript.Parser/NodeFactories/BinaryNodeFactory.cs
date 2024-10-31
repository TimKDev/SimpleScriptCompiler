using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories;

public abstract class BinaryNodeFactory<TNode, TNodeArg>
{
    protected abstract Result<TNode> Factory(TNodeArg firstOperand, TNodeArg secondOperant);

    public Result<TNode> Create(List<Token> firstOperand, List<Token> secondOperand,
        IExpressionFactory expressionFactory)
    {
        List<Error> errors = [];
        var firstValueResult =
            expressionFactory.Create(firstOperand).MapIfSuccess(ConvertToNodeArg);
        var secondValueResult =
            expressionFactory.Create(secondOperand).MapIfSuccess(ConvertToNodeArg);

        errors.AddRange(firstValueResult.Errors);
        errors.AddRange(secondValueResult.Errors);

        if (errors.Any())
        {
            return errors;
        }

        return errors.Any() ? errors : Factory(firstValueResult.Value, secondValueResult.Value);
    }

    private static Result<TNodeArg> ConvertToNodeArg(IExpression expression) =>
        expression is TNodeArg nodeArg
            ? Result.ToResult(nodeArg)
            : expression.CreateError($"Expression is not supported for {typeof(TNodeArg).Name}.");
}
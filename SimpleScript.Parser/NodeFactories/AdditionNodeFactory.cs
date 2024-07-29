using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories
{
    public class AdditionNodeFactory : IAdditionNodeFactory
    {
        public AdditionNodeFactory() { }
        public Result<AddNode> Create(List<Token> firstOperand, List<Token> secondOperand, IExpressionFactory expressionFactory)
        {
            List<Error> errors = [];
            Result<IAddable> firstValueResult = expressionFactory.Create(firstOperand).MapIfSuccess(ConvertToAddable);
            Result<IAddable> secondValueResult = expressionFactory.Create(secondOperand).MapIfSuccess(ConvertToAddable);

            errors.AddRange(firstValueResult.Errors);
            errors.AddRange(secondValueResult.Errors);

            return errors.Any() ? errors : AddNode.Create(firstValueResult.Value, secondValueResult.Value);
        }

        private static Result<IAddable> ConvertToAddable(IExpression expression) => expression is IAddable addable ? Result.ToResult(addable) : expression.CreateError("Expression is not supported for addition.");
    }
}

using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Extensions;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories
{
    public class MultiplicationNodeFactory : IMultiplicationNodeFactory
    {
        public Result<MultiplyNode> Create(List<Token> firstOperand, List<Token> secondOperand, IExpressionFactory expressionFactory)
        {
            List<Error> errors = [];
            Result<IMultiplyable> firstValueResult = expressionFactory.Create(firstOperand).MapIfSuccess(ConvertToMultiplyable);
            Result<IMultiplyable> secondValueResult = expressionFactory.Create(secondOperand).MapIfSuccess(ConvertToMultiplyable);

            errors.AddRange(firstValueResult.Errors);
            errors.AddRange(secondValueResult.Errors);

            if (errors.Any())
            {
                return errors;
            }

            return errors.Any() ? errors : MultiplyNode.Create(firstValueResult.Value, secondValueResult.Value);
        }

        private static Result<IMultiplyable> ConvertToMultiplyable(IExpression expression) => expression is IMultiplyable multiplyable ? ResultExtensions.ConvertTypeToResult(multiplyable) : expression.CreateError("Expression is not supported for multiplication.");
    }
}

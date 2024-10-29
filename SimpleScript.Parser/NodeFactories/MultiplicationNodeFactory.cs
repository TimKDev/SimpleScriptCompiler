using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.NodeFactories
{
    public class EqualiltyNodeFactory : IEqualiltyNodeFactory
    {
        public Result<EqulityNode> Create(List<Token> firstOperand, List<Token> secondOperand,
            IExpressionFactory expressionFactory)
        {
            List<Error> errors = [];
            var firstValueResult =
                expressionFactory.Create(firstOperand).MapIfSuccess(ConvertToEqualizable);
            var secondValueResult =
                expressionFactory.Create(secondOperand).MapIfSuccess(ConvertToEqualizable);

            errors.AddRange(firstValueResult.Errors);
            errors.AddRange(secondValueResult.Errors);

            if (errors.Any())
            {
                return errors;
            }

            return errors.Any() ? errors : EqulityNode.Create(firstValueResult.Value, secondValueResult.Value);
        }

        private static Result<IEqualizable> ConvertToEqualizable(IExpression expression) =>
            expression is IEqualizable equalizable
                ? Result.ToResult(equalizable)
                : expression.CreateError("Expression is not supported for equality.");
    }

    public class InEqualiltyNodeFactory : BinaryNodeFactory<InEqulityNode, IEqualizable>, IInEqualiltyNodeFactory
    {
        protected override Result<InEqulityNode> Factory(IEqualizable firstOperand, IEqualizable secondOperant)
        {
            return InEqulityNode.Create(firstOperand, secondOperant);
        }
    }

    public abstract class BinaryNodeFactory<TNode, TNodeArg>
    {
        protected abstract Result<TNode> Factory(TNodeArg firstOperand, TNodeArg secondOperant);

        public Result<TNode> Create(List<Token> firstOperand, List<Token> secondOperand,
            IExpressionFactory expressionFactory)
        {
            List<Error> errors = [];
            var firstValueResult =
                expressionFactory.Create(firstOperand).MapIfSuccess(ConvertToEqualizable);
            var secondValueResult =
                expressionFactory.Create(secondOperand).MapIfSuccess(ConvertToEqualizable);

            errors.AddRange(firstValueResult.Errors);
            errors.AddRange(secondValueResult.Errors);

            if (errors.Any())
            {
                return errors;
            }

            return errors.Any() ? errors : Factory(firstValueResult.Value, secondValueResult.Value);
        }

        private static Result<TNodeArg> ConvertToEqualizable(IExpression expression) =>
            expression is TNodeArg sizeComparable
                ? Result.ToResult(sizeComparable)
                : expression.CreateError($"Expression is not supported for {typeof(TNodeArg).Name}.");
    }


    public interface IInEqualiltyNodeFactory
    {
        Result<InEqulityNode> Create(List<Token> firstOperand, List<Token> secondOperand,
            IExpressionFactory expressionFactory);
    }

    public interface IEqualiltyNodeFactory
    {
        Result<EqulityNode> Create(List<Token> firstOperand, List<Token> secondOperand,
            IExpressionFactory expressionFactory);
    }

    public class MultiplicationNodeFactory : IMultiplicationNodeFactory
    {
        public Result<MultiplyNode> Create(List<Token> firstOperand, List<Token> secondOperand,
            IExpressionFactory expressionFactory)
        {
            List<Error> errors = [];
            Result<IMultiplyable> firstValueResult =
                expressionFactory.Create(firstOperand).MapIfSuccess(ConvertToMultiplyable);
            Result<IMultiplyable> secondValueResult =
                expressionFactory.Create(secondOperand).MapIfSuccess(ConvertToMultiplyable);

            errors.AddRange(firstValueResult.Errors);
            errors.AddRange(secondValueResult.Errors);

            if (errors.Any())
            {
                return errors;
            }

            return errors.Any() ? errors : MultiplyNode.Create(firstValueResult.Value, secondValueResult.Value);
        }

        private static Result<IMultiplyable> ConvertToMultiplyable(IExpression expression) =>
            expression is IMultiplyable multiplyable
                ? Result.ToResult(multiplyable)
                : expression.CreateError("Expression is not supported for multiplication.");
    }
}
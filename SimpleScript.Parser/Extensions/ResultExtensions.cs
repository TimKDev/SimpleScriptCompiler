using EntertainingErrors;

namespace SimpleScript.Parser.Extensions
{
    public static class ResultExtensions
    {
        public static Result<T> ConvertTypeToResult<T>(T value)
        {
            return value;
        }

        public static Result<TResult> MapIfSuccess<TValue, TResult>(this Result<TValue> result, Func<TValue, Result<TResult>> mapFunc)
        {
            if (result.IsSuccess)
            {
                Result<TResult> mappedResult = mapFunc(result.Value);
                if (mappedResult.IsSuccess)
                {
                    return mappedResult;
                }
                result.Errors.AddRange(mappedResult.Errors);
            }

            return result.Errors;
        }
    }
}
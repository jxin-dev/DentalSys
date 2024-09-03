using Newtonsoft.Json;

namespace DentalSys.Api.Common.Models
{
    public class Result
    {
        public Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
        public bool IsSuccess { get; private set; }
        public Error Error { get; private set; }

        public bool IsFailure => !IsSuccess;

        public static Result Success()
        {
            return new Result(true, null!);
        }
        public static Result Failure(Error error)
        {
            return new Result(false, error);
        }

        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new Result<TValue>(value, true, null!);
        }

        public static Result<TValue> Failure<TValue>(Error error)
        {
            return new Result<TValue>(default!, false, error);
        }
    }
    public class Result<TValue> : Result
    {
        public TValue Value { get; private set; }
        public Result(TValue value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            Value = value;
        }
    }
    public static class ResultExtensions
    {
        public static T Match<T>(this Result result, Func<T> onSuccess, Func<Error, T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Error);
        }
    }

    public class CustomProblemResult : IResult
    {
        private Error Error { get; }
        public CustomProblemResult(Error error)
        {
            Error = error;
        }
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = Error.StatusCode;

            var response = JsonConvert.SerializeObject(new { code = Error.Code, description = Error.Description });
            await httpContext.Response.WriteAsync(response);
        }
    }
}

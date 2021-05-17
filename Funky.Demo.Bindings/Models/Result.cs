using System.Runtime.CompilerServices;

namespace Funky.Demo.Models
{
    public class Result
    {
        public string Error { get; set; }

        public bool Status => string.IsNullOrWhiteSpace(Error);

        public static Result Failure(string error)
        {
            return new Result
            {
                Error = error
            };
        }

        public static Result Success()
        {
            return new Result
            {
                Error = string.Empty
            };
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public new static Result<T> Failure(string error)
        {
            return new Result<T>
            {
                Error = error
            };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                Data = data
            };
        }
    }
}
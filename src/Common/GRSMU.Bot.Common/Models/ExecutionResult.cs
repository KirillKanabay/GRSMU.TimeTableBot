namespace GRSMU.Bot.Common.Models
{
    public record ExecutionResult
    {
        public ExecutionResult()
        {
        }

        public ExecutionResult(Error error)
        {
            Error = error;
        }

        public bool IsSuccess => !HasErrors;

        public bool HasErrors => Error != null;

        public Error? Error { get; }

        public static ExecutionResult Failure(Error error)
        {
            return new ExecutionResult(error);
        }

        public static ExecutionResult<T> Failure<T>(Error error)
        {
            return new ExecutionResult<T>(error);
        }

        public static ExecutionResult Success()
        {
            return new ExecutionResult();
        }

        public static ExecutionResult<T> Success<T>(T data)
        {
            return new ExecutionResult<T>(data);
        }
    }

    public record ExecutionResult<T> : ExecutionResult
    {
        private readonly T? _data;

        public ExecutionResult(Error error) : base(error)
        {
        }

        public ExecutionResult(T data)
        {
            _data = data;
        }

        public T Data => IsSuccess
            ? _data!
            : throw new InvalidOperationException("The value of a failure result can't be accessed.");

        public new static ExecutionResult<T> Failure(Error error)
        {
            return new ExecutionResult<T>(error);
        }
    }
}

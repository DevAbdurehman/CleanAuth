namespace CleanAuth.Application.Common;

public class Result
{
    public bool IsSuccess { get; private set; }

    public string Message { get; private set; } = string.Empty;

    public object? Data { get; private set; }

    protected Result(
        bool isSuccess,
        string message,
        object? data = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
    }

    public static Result Success(string message)
    {
        return new Result(true, message);
    }

    public static Result Success(object data)
    {
        return new Result(true, string.Empty, data);
    }

    public static Result Failure(string message)
    {
        return new Result(false, message);
    }
}
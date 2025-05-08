namespace Blog.Application.Common.Models;

public class Result<T> where T : class
{
    public Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public Result(bool succeeded, IEnumerable<string> errors, T data)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Data = data;
    }

    public bool Succeeded { get; init; }

    public string[] Errors { get; init; }

    public T? Data { get; init; }

    public static Result<T> Success()
    {
        return new Result<T>(true, Array.Empty<string>());
    }

    public static Result<T> Success(T data)
    {
        return new Result<T>(true, Array.Empty<string>(), data);
    }

    public static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>(false, errors);
    }

    public static Result<T> Failure(IEnumerable<string> errors, T data)
    {
        return new Result<T>(false, errors, data);
    }
}

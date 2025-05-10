using Blog.Domain.Enums;

namespace Blog.Application.Common.Models;

public class Result<T> where T : class
{
    public Result(bool succeeded, ResultStatus status, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Status = status;
    }

    public Result(bool succeeded, ResultStatus status, IEnumerable<string> errors, T data)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Data = data;
        Status = status;
    }

    public bool Succeeded { get; init; }

    public string[] Errors { get; init; }

    public T? Data { get; init; }

    public ResultStatus Status { get; set; }

    public static Result<T> Success()
    {
        return new Result<T>(true, ResultStatus.Success, Array.Empty<string>());
    }

    public static Result<T> Success(T data)
    {
        return new Result<T>(true, ResultStatus.Success, Array.Empty<string>(), data);
    }

    public static Result<T> Failure(IEnumerable<string> errors, ResultStatus? status = null)
    {
        return new Result<T>(false, status ?? ResultStatus.Failure, errors);
    }

    public static Result<T> Failure(IEnumerable<string> errors, T data, ResultStatus? status = null)
    {
        return new Result<T>(false, status ?? ResultStatus.Failure, errors, data);
    }
}

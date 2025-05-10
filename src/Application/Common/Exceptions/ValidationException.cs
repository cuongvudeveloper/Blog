using FluentValidation.Results;

namespace Blog.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
        FlattenedErrors = [];
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

        FlattenedErrors = failures.Select(f => f.ErrorMessage).ToArray();
    }

    public IDictionary<string, string[]> Errors { get; }
    public string[] FlattenedErrors { get; }
}

namespace FitLog.Application.Common.Models;

public class Result
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Success = succeeded;
        Errors = errors.ToArray();
    }

    public bool Success { get; init; }

    public string[] Errors { get; init; }

    public static Result Successful()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}

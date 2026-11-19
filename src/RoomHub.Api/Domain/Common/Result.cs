namespace RoomHub.Api.Domain.Common;

public sealed class Result<T>
{
    private readonly T _value;
    private readonly Error _error;

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public T Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Cannot access Value on a failed result.");

    public Error Error => IsFailure
        ? _error
        : throw new InvalidOperationException("Cannot access Error on a successful result.");

    private Result(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        IsSuccess = true;
        _value = value;
        _error = null!;
    }

    private Result(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        IsSuccess = false;
        _value = default!;
        _error = error;
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(Error error) => new(error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess ? onSuccess(_value) : onFailure(_error);
    }
}

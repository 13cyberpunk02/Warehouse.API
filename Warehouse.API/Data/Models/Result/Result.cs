using Warehouse.API.Data.Models.Error;
namespace Warehouse.API.Data.Models.Result;

public class Result
{
    protected bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ErrorResponse Error { get; }

    protected Result(bool success, ErrorResponse error)
    {
        if ((success && error != ErrorResponse.None) || (!success && error == ErrorResponse.None))
        {
            throw new InvalidOperationException("Невозможно выполнить операцию");
        }

        IsSuccess = success;
        Error = error;
    }

    public static Result Success() => new(true, ErrorResponse.None);
    public static ResultT<TValue> Success<TValue>(TValue value) => new(value, true, ErrorResponse.None);
    public static Result Failure(ErrorResponse error) => new(false, error);
}
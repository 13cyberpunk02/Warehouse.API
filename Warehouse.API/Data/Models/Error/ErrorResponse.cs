namespace Warehouse.API.Data.Models.Error;

public sealed record ErrorResponse(string Code, string Message)
{
    internal static ErrorResponse None => new(ErrorTypeConstant.None, string.Empty);
}
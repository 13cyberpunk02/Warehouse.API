namespace Warehouse.API.Data.Models.Error.ErrorTypes;

public static class ErrorsCollection
{
    public static ErrorResponse ErrorCollection(IEnumerable<string> errors) =>
        new(ErrorTypeConstant.BadRequest, string.Join(Environment.NewLine, errors));
}
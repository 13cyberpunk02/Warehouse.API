using Warehouse.API.Data.Models.Error;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Extensions;

public static class ResultExtension
{
    public static IResult ToHttpResponse(this Result result)
    {
        if (result.IsFailure)
            return MapErrorResponse(result.Error, result);
        return Results.Ok(result);
    }

    public static IResult FromHttpResponse<T>(this ResultT<T> result)
    {
        if (result.IsFailure)
            return MapErrorResponse(result.Error, result);
        return Results.Ok(result);
    }

    private static IResult MapErrorResponse(ErrorResponse error, object result)
    {
        return error.Code switch
        {
            ErrorTypeConstant.ValidationError => Results.BadRequest(result),
            ErrorTypeConstant.UnrecognizedRequestError => Results.BadRequest(result),
            ErrorTypeConstant.BadRequest => Results.BadRequest(result),
            ErrorTypeConstant.NotFound => Results.NotFound(result),
            ErrorTypeConstant.Forbidden => Results.Forbid(),
            ErrorTypeConstant.UnAuthorized => Results.Unauthorized(),
            _ => Results.Problem(detail: error.Message, statusCode: 500, title: ErrorTypeConstant.InternalServerError)
        };
    }
}
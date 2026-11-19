namespace Warehouse.API.Data.Models.Error.ErrorTypes.CartridgeErrors;

public static class CartridgeErrors
{ 
    public static ErrorResponse CartridgeNotFound => new(ErrorTypeConstant.NotFound, "Картридж не найден");
    public static ErrorResponse CartridgesTableEmpty => new(ErrorTypeConstant.NotFound, "Картриджи не созданы, добавьте хотя бы одну модель картриджа");
    public static ErrorResponse CartridgeExists => new(ErrorTypeConstant.BadRequest, "Такая модель картриджа уже есть в списке");
    public static ErrorResponse CartridgeDidntSave =>
            new(ErrorTypeConstant.InternalServerError, "Произошла ошибка при записи в БД");
}
namespace Warehouse.API.Data.Models.Error.ErrorTypes.PrinterErrors;

public static class PrinterErrors
{
    public static ErrorResponse PrinterNotFound => new(ErrorTypeConstant.NotFound, "Принтер не найден");
    public static ErrorResponse PrinterExists => new(ErrorTypeConstant.BadRequest, "Такая модель принтера уже есть в справочнике");
    public static ErrorResponse CartridgesNotFound => new(ErrorTypeConstant.BadRequest, "Некоторые из выбранных картриджей не найдены");
}

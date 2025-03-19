namespace Warehouse.API.Data.Models.Error.ErrorTypes.PaperErrors;

public static class PaperErrors
{
    public static ErrorResponse PaperNotFound => new(ErrorTypeConstant.NotFound, "Бумага не найдена");
    public static ErrorResponse PapersNotFound => new(ErrorTypeConstant.NotFound, "В БД нет записи про бумаг, добавьте хотя бы одну");
    public static ErrorResponse PaperExists => new(ErrorTypeConstant.BadRequest, "Бумага уже есть в списке записей");

    public static ErrorResponse PaperDidntSave =>
        new(ErrorTypeConstant.InternalServerError, "Произошла ошибка при записи в БД");
}
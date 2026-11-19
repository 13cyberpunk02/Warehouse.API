namespace Warehouse.API.Data.Models.Error.ErrorTypes.PaperErrors;

public static class PaperErrors
{
    public static ErrorResponse PaperNotFound => new(ErrorTypeConstant.NotFound, "Бумага не найдена");
    public static ErrorResponse PapersTableEmpty => new(ErrorTypeConstant.NotFound, "В базе данных нет информации о бумагах, скорее не создано ни одной записи, проверьте и добавьте хотя бы одну запись");
    public static ErrorResponse PaperExists => new(ErrorTypeConstant.BadRequest, "Бумага уже есть в списке записей");

    public static ErrorResponse PaperDidntSave =>
        new(ErrorTypeConstant.InternalServerError, "Произошла ошибка при записи в БД");
}
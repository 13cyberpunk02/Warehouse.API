namespace Warehouse.API.Data.Models.Error.ErrorTypes.DeliveryErrors;

public static class DeliveryErrors
{
    public static ErrorResponse DeliveryNotFound => new(ErrorTypeConstant.NotFound, "Выдача бумаг не найдена");
    public static ErrorResponse PaperIsNotFound => new(ErrorTypeConstant.NotFound, "Указанная бумага для выдачи не найдена");
    public static ErrorResponse DeliveryPaperQuantityIsGreater => new(ErrorTypeConstant.BadRequest, "Количество для выдачи бумаг превышает количество бумаг на складе");
    public static ErrorResponse UserIdAndDepartmentIdNotProvided => 
        new(ErrorTypeConstant.BadRequest, "Id пользователя и отдела не указано");
    
    public static ErrorResponse AuthorizedUserTokenNotProvided => 
        new(ErrorTypeConstant.BadRequest, "Вы не авторизованы, для добавления данных");
    
    public static ErrorResponse DeliverySaveError => 
        new(ErrorTypeConstant.InternalServerError, "Ошибка при сохранении данных");
}
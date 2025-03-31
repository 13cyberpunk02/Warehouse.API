namespace Warehouse.API.Data.Models.Error.ErrorTypes.DepartmentErrors;

public static class DepartmentErrors
{
    public static ErrorResponse DepartmentsIsEmpty => 
        new(ErrorTypeConstant.NotFound, "Нет записей об отделе в БД, добавьте хотя бы одну");
    
    public static ErrorResponse DepartmentsAddNameError => 
        new(ErrorTypeConstant.NotFound, "Вы не указали имя отдела");
    public static ErrorResponse DepartmentNotFound => 
        new(ErrorTypeConstant.NotFound, "Отдел не найден");

    
    public static ErrorResponse DepartmentExistsError => 
        new(ErrorTypeConstant.BadRequest, "Отдел существует");
    public static ErrorResponse DepartmentCantRemove =>
        new(ErrorTypeConstant.InternalServerError, "Произошла ошибка при удалении записи в БД");
    public static ErrorResponse DepartmentDidntSave =>
        new(ErrorTypeConstant.InternalServerError, "Произошла ошибка при записи в БД");
}
namespace Warehouse.API.Data.Models.Error.ErrorTypes.AccountErrors;

public static class AccountErrors
{
    public static ErrorResponse EmptyUsersListError => 
        new (ErrorTypeConstant.NotFound, "Отсутствуют пользователи в базе данных, нужно хотя бы создать одну");
    public static ErrorResponse UserNotFound => 
        new(ErrorTypeConstant.NotFound, "Пользователь не найден");
    public static ErrorResponse UserIdNotProvided => 
        new(ErrorTypeConstant.BadRequest, "Id пользователя не был передан");
    public static ErrorResponse UserEmailNotProvided => 
        new(ErrorTypeConstant.BadRequest, "Эл. почта пользователя не был передан");
    public static ErrorResponse UserForUpdateNotFound => 
        new(ErrorTypeConstant.BadRequest, "Пользователь для обновления не найден");
    public static ErrorResponse RoleNotFound => 
        new(ErrorTypeConstant.BadRequest, "Роль не найден");
    public static ErrorResponse UserAlreadyHasProvidedRole => 
        new(ErrorTypeConstant.BadRequest, "Пользователь уже находится в данной роли");
}
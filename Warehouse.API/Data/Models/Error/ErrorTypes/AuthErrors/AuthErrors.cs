namespace Warehouse.API.Data.Models.Error.ErrorTypes.AuthErrors;

public static class AuthErrors
{
    public static ErrorResponse InvalidRegistrationRequest => new (ErrorTypeConstant.ValidationError, "Неправильно заполнены данные в запросе");
    public static ErrorResponse UserAlreadyExists => new(ErrorTypeConstant.ValidationError, "Пользователь с данной эл. почтой уже существует");
    public static ErrorResponse InvalidLoginRequest => new(ErrorTypeConstant.NotFound, "Неправильный логин или пароль");
    public static ErrorResponse UserNotFound => new(ErrorTypeConstant.NotFound, "Пользователь не найден");
    public static ErrorResponse EmailEmptyOrNull => new(ErrorTypeConstant.NotFound, "Эл. почта пустая");
    public static ErrorResponse InvalidRefreshToken => new(ErrorTypeConstant.BadRequest, "Неверный токен обновления, авторизуйтесь заново");
    public static ErrorResponse RefreshTokenExpired => new(ErrorTypeConstant.BadRequest, "Срок токена обновления истек, авторизуйтесь заново");
    public static ErrorResponse InvalidRefreshTokenExpirationDate => new(ErrorTypeConstant.BadRequest, "Неверное время токена обновления");
    public static ErrorResponse ErrorRequest => new(ErrorTypeConstant.BadRequest, "Отправлен неправильный запрос с клиента.");
}
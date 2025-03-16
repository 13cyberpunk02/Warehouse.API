namespace Warehouse.API.Data.Models.DTO_s.Responses.Authentication;

public record LoginResponse(
    string AccessToken,
    string RefreshToken);
namespace Warehouse.API.Data.Models.DTO_s.Responses.Account;

public record UsersResponse(
    string Id, 
    string? Email, 
    string Fullname, 
    string? AvatarImageUrl,
    int DepartmentId,
    string Department);
namespace Warehouse.API.Data.Models.DTO_s.Responses.Account;

public record UsersResponse(
    string Id, 
    string? UserName, 
    string Fullname, 
    string Firstname, 
    string Lastname,
    int? DepartmentId,
    string? Department,
    string? RoleName);
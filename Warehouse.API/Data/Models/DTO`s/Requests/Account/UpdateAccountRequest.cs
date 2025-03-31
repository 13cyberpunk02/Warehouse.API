namespace Warehouse.API.Data.Models.DTO_s.Requests.Account;

public record UpdateAccountRequest(
    string Id,
    string Firstname,
    string Lastname,
    string UserName,
    int DepartmentId,
    string RoleName);
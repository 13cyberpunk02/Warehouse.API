namespace Warehouse.API.Data.Models.DTO_s.Requests.Account;

public record UpdateAccountRequest(
    string Firstname,
    string Lastname,
    string Email,
    string AvatarImageUrl,
    int DepartmentId);
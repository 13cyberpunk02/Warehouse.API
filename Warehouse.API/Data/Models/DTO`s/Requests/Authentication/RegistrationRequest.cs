namespace Warehouse.API.Data.Models.DTO_s.Requests.Authentication;

public record RegistrationRequest(
    string Username,
    string Firstname,
    string Lastname,
    string Password,
    string RoleName,
    string ConfirmPassword,
    int DepartmentId);

    
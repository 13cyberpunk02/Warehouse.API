namespace Warehouse.API.Data.Models.DTO_s.Requests.Authentication;

public record RegistrationRequest(
    string Email,
    string Firstname,
    string Lastname,
    string Password,
    string ConfirmPassword,
    int DepartmentId);

    
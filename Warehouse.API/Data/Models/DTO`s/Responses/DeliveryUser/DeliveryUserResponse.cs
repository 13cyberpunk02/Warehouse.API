namespace Warehouse.API.Data.Models.DTO_s.Responses.DeliveryUser;

public record DeliveryUserResponse(
    int Id,
    string Fullname, 
    string Firstname, 
    string Lastname,
    string? Patronymic,
    int? DepartmentId,
    string? Department);
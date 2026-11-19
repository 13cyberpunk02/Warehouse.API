namespace Warehouse.API.Data.Models.DTO_s.Requests.DeliveryUser;

public record UpdateDeliveryUserRequest(
    int Id,
    string Firstname,
    string Lastname,
    string Patronymic,
    int DepartmentId);
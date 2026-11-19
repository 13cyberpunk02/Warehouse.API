namespace Warehouse.API.Data.Entities;

public class DeliveryUser
{
    public int Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public string Fullname => Lastname + " " + Firstname + " " + Patronymic;
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
}
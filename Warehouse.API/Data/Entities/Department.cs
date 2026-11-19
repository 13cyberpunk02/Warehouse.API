using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Data.Entities;

public class Department
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<DeliveryUser> DeliveryUsers { get; set; } = null!;
}
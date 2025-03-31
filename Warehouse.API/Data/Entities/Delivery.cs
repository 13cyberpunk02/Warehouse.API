using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Data.Entities;

public class Delivery
{
    [Key]
    public int Id { get; set; }
    public DateTime DeliveredAt { get; set; } = DateTime.Now;
    public string DeliveredUser { get; set; } = string.Empty;
    public string ReceivedUser { get; set; } = string.Empty;
    public string ReceivedUserDepartment { get; set; } = string.Empty;
    public string PaperFormat { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
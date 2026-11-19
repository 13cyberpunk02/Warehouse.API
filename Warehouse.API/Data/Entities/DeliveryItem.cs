using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Data.Entities;

public class DeliveryItem
{
    [Required, Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Delivery? Delivery { get; set; }
}
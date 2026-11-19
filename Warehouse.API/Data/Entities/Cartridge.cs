using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Data.Entities;

public class Cartridge
{
    [Key, Required]
    public int Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
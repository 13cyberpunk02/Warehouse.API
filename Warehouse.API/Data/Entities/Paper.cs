using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Data.Entities;

public class Paper
{
    [Key]
    public int Id { get; set; }
    public string Format { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
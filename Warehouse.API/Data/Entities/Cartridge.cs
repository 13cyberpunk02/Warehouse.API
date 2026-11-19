using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Warehouse.API.Data.Entities;

public class Cartridge
{
    [Key, Required]
    public int Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Quantity { get; set; }

    /// <summary>Принтеры, к которым подходит картридж (многие ко многим)</summary>
    [JsonIgnore]
    public List<Printer> Printers { get; set; } = [];
}

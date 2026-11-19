using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Data.Entities;

/// <summary>Модель принтера из справочника. Нужен, чтобы при выдаче искать картридж по принтеру.</summary>
public class Printer
{
    [Key]
    public int Id { get; set; }
    public string Model { get; set; } = string.Empty;
    /// <summary>Необязательная заметка: где стоит, чей и т.п.</summary>
    public string Note { get; set; } = string.Empty;

    /// <summary>Подходящие картриджи (многие ко многим)</summary>
    public List<Cartridge> Cartridges { get; set; } = [];
}

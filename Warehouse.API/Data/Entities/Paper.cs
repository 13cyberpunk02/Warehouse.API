using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Data.Entities;

public class Paper
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Наименование бумаги обязательно к заполнению")]
    [MaxLength(100, ErrorMessage = "Наименование бумаги может содержать не более 100 букв")]
    [MinLength(2, ErrorMessage = "Наименование бумаги может содержать не менее 2 букв")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Тип бумаги обязательно к заполнению")]
    [MaxLength(100, ErrorMessage = "Тип бумаги может содержать не более 100 букв")]
    [MinLength(2, ErrorMessage = "Тип бумаги может содержать не менее 2 букв")]
    public string Type { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Формат бумаги обязательно к заполнению")]
    [MaxLength(100, ErrorMessage = "Формат бумаги может содержать не более 100 букв")]
    [MinLength(2, ErrorMessage = "Формат бумаги может содержать не менее 2 букв")]
    public string Format { get; set; } = string.Empty;

    [Required(ErrorMessage = "Количество бумаги обязательно к заполнению")]
    [MaxLength(10000, ErrorMessage = "Количество бумаги может быть не более 100 штук")]
    [MinLength(1, ErrorMessage = "Количество бумаги может быть не менее 1 штуки")]
    public int Quantity { get; set; }
}
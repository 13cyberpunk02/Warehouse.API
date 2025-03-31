using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Data.Entities;

public class Department
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Наименование отдела обязательна к заполнению")]
    [MaxLength(100, ErrorMessage = "Наименование отдела может содержать не более 100 букв")]
    [MinLength(3, ErrorMessage = "Наименование отдела может содержать не менее 3 букв")]
    public string Name { get; set; } = string.Empty;

    public List<AppUser> Employees { get; set; } = new();
}
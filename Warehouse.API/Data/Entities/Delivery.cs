using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.API.Data.Entities;

public class Delivery
{
    [Key]
    public int Id { get; set; }

    public DateTime DeliveredAt { get; set; } = DateTime.Now;

    public string DeliveredUserId { get; set; } = string.Empty;
    [ForeignKey("DeliveredUserId")]
    public AppUser DeliveredUser { get; set; } = null!;
    
    
    public string ReceivedUserId { get; set; } = string.Empty;
    [ForeignKey("ReceivedUserId")]
    public AppUser ReceivedUser { get; set; } = null!;

    public int PaperId { get; set; }
    [ForeignKey("PaperId")]
    public Paper Paper { get; set; } = null!;

    [Required(ErrorMessage = "Количество для выдачи бумаги не может быть пустым")]
    [MaxLength(100, ErrorMessage = "Количество для выдачи бумаги не может превышать 100 штук")]
    [MinLength(1, ErrorMessage = "Количество для выдачи бумаги не может быть 0 штук")]
    public int Quantity { get; set; }
}
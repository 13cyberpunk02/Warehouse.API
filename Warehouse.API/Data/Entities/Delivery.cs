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
    
    
    public string ReceivedUserId { get; set; }
    [ForeignKey("ReceivedUserId")]
    public AppUser ReceivedUser { get; set; } = null!;

    public string PaperFormat { get; set; } = string.Empty;
    
    public int Quantity { get; set; }
}
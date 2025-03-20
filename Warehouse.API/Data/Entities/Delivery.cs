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

    public int PaperId { get; set; }
    [ForeignKey("PaperId")]
    public Paper Paper { get; set; } = null!;
    
    public int Quantity { get; set; }
}
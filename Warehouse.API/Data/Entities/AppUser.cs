using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Warehouse.API.Data.Entities;

public class AppUser : IdentityUser
{
    public string Firstname { get; set; } = string.Empty;
    
    public string Lastname { get; set; } = string.Empty;
    public string Fullname => Firstname + " " + Lastname;
    public string? AvatarImageUrl { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    [ForeignKey("DepartmentId")] 
    public Department Department { get; set; } = null!;

    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset RefreshTokenExpiry { get; set; }
}
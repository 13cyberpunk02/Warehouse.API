using Microsoft.AspNetCore.Identity;

namespace Warehouse.API.Data.Entities;

public class AppUser : IdentityUser
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Fullname => Firstname + " " + Lastname;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset RefreshTokenExpiry { get; set; }
}
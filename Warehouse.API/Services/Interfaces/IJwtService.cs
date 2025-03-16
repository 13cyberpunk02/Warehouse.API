using System.Security.Claims;
using Warehouse.API.Data.Entities;

namespace Warehouse.API.Services.Interfaces;

public interface IJwtService
{
    Task<string> GenerateJwtTokenAsync(AppUser user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
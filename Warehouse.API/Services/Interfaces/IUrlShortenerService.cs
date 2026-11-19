using Warehouse.API.Data.Entities;

namespace Warehouse.API.Services.Interfaces;

public interface IUrlShortenerService
{
    Task<ShortUrl?> GetByShortCodeAsync(string shortCode);
    Task<ShortUrl> CreateAsync(ShortUrl shortUrl);
    Task<bool> ShortCodeExistsAsync(string shortCode);
    Task<ShortUrl> CreateShortUrlAsync(string originalUrl);
    Task<string?> GetOriginalUrlAsync(string shortCode);
}
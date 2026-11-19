using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly DataContext _context;

    public UrlShortenerService(DataContext context)
    {
        _context = context;
    }
    public async Task<ShortUrl?> GetByShortCodeAsync(string shortCode)
    {
        return await _context.ShortUrls
            .FirstOrDefaultAsync(x => x.ShortCode == shortCode);
    }

    public async Task<ShortUrl> CreateAsync(ShortUrl shortUrl)
    {
        await _context.ShortUrls.AddAsync(shortUrl);
        await _context.SaveChangesAsync();
        return shortUrl;
    }

    public async Task<ShortUrl> IncrementVisitCountAsync(int id)
    {
        var shortUrl = await _context.ShortUrls.FindAsync(id);
        if (shortUrl is not null)
        {
            shortUrl.VisitCount++;
            await _context.SaveChangesAsync();
        }
        return shortUrl!;
    }

    public async Task<bool> ShortCodeExistsAsync(string shortCode)
    {
        return await _context.ShortUrls.AnyAsync(x => x.ShortCode == shortCode);
    }

    public async Task<ShortUrl> CreateShortUrlAsync(string originalUrl)
    {
        string shortCode = await GenerateUniqueShortCodeAsync(originalUrl);
        
        var shortUrl = new ShortUrl
        {
            OriginalUrl = originalUrl,
            ShortCode = shortCode,
            CreatedAt = DateTime.UtcNow,
            VisitCount = 0
        };
        
        return await CreateAsync(shortUrl);
    }

    public async Task<string?> GetOriginalUrlAsync(string shortCode)
    {
        var shortUrl = await GetByShortCodeAsync(shortCode);
        if (shortUrl == null) return null;

        await IncrementVisitCountAsync(shortUrl.Id);
        return shortUrl.OriginalUrl;
    }
    
    private async Task<string> GenerateUniqueShortCodeAsync(string url)
    {
        using var sha256 = SHA256.Create();
        var attempt = 0;
        string shortCode;

        do
        {
            var input = $"{url}{attempt}";
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            shortCode = Convert.ToBase64String(hash)
                .Replace("/", "_")
                .Replace("+", "-")
                .Substring(0, 7);
            attempt++;
        } while (await ShortCodeExistsAsync(shortCode));

        return shortCode;
    }
}
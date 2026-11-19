using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class LocalFileStorageService : IFileStorageService
{
      private readonly string _storagePath;
        private readonly DataContext _context;

        public LocalFileStorageService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _storagePath = configuration["Storage:Path"] ?? Path.Combine(Directory.GetCurrentDirectory(), "Storage");
            Directory.CreateDirectory(_storagePath);
        }

        public async Task<ProcessedFile> SaveFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var shortCode = await GenerateShortCodeAsync();
            var filePath = Path.Combine(_storagePath, shortCode);

            await using (var fileStream2 = File.Create(filePath))
            {
                await fileStream.CopyToAsync(fileStream2);
            }

            var processedFile = new ProcessedFile
            {
                OriginalFileName = fileName,
                StoragePath = filePath,
                ShortCode = shortCode,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                FileSize = new FileInfo(filePath).Length,
                ContentType = contentType
            };

            _context.ProcessedFiles.Add(processedFile);
            await _context.SaveChangesAsync();

            return processedFile;
        }

        public async Task<Stream> GetFileAsync(string shortCode)
        {
            var fileInfo = await GetFileInfoAsync(shortCode);
            if (fileInfo == null || !File.Exists(fileInfo.StoragePath))
            {
                throw new FileNotFoundException();
            }

            return File.OpenRead(fileInfo.StoragePath);
        }

        public async Task<bool> DeleteFileAsync(string shortCode)
        {
            var fileInfo = await GetFileInfoAsync(shortCode);
            if (fileInfo == null)
            {
                return false;
            }

            if (File.Exists(fileInfo.StoragePath))
            {
                File.Delete(fileInfo.StoragePath);
            }

            _context.ProcessedFiles.Remove(fileInfo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> GenerateShortCodeAsync()
        {
            using var sha256 = SHA256.Create();
            var attempt = 0;
            string shortCode;

            do
            {
                var input = $"{Guid.NewGuid()}{attempt}";
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                shortCode = Convert.ToBase64String(hash)
                    .Replace("/", "_")
                    .Replace("+", "-")
                    .Substring(0, 7);
                attempt++;
            } while (await _context.ProcessedFiles.AnyAsync(x => x.ShortCode == shortCode));

            return shortCode;
        }

        public async Task<ProcessedFile?> GetFileInfoAsync(string shortCode)
        {
            return await _context.ProcessedFiles.FirstOrDefaultAsync(x => x.ShortCode == shortCode);
        }
}
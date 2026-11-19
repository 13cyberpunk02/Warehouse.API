using Warehouse.API.Data.Entities;

namespace Warehouse.API.Services.Interfaces;

public interface IFileStorageService
{
    Task<ProcessedFile> SaveFileAsync(Stream fileStream, string fileName, string contentType);
    Task<Stream> GetFileAsync(string shortCode);
    Task<bool> DeleteFileAsync(string shortCode);
    Task<string> GenerateShortCodeAsync();
    Task<ProcessedFile?> GetFileInfoAsync(string shortCode);
}
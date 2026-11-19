namespace Warehouse.API.Data.Entities;

public class ProcessedFile
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
}
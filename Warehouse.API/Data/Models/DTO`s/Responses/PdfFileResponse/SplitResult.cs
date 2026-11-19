namespace Warehouse.API.Data.Models.DTO_s.Responses.PdfFileResponse;

public record SplitResult(string ArchivePath, int PageCount, long ArchiveSize);
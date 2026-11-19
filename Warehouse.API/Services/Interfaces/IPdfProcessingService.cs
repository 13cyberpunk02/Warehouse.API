namespace Warehouse.API.Services.Interfaces;

public interface IPdfProcessingService
{
    /// <summary>Разбивает PDF на отдельные страницы и возвращает ZIP-архив с ними.</summary>
    Task<byte[]> SplitPdfToZipAsync(Stream pdfStream, string originalFileName, CancellationToken ct = default);
}

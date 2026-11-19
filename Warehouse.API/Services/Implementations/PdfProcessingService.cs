using System.IO.Compression;
using iText.Kernel.Pdf;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class PdfProcessingService : IPdfProcessingService
{
    public async Task<byte[]> SplitPdfToZipAsync(Stream pdfStream, string originalFileName, CancellationToken ct = default)
    {
        // iText читает документ синхронно, поэтому сначала буферизуем поток загрузки
        using var input = new MemoryStream();
        await pdfStream.CopyToAsync(input, ct);
        input.Position = 0;

        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        if (string.IsNullOrWhiteSpace(baseName))
            baseName = "document";

        using var zipBuffer = new MemoryStream();
        using (var archive = new ZipArchive(zipBuffer, ZipArchiveMode.Create, leaveOpen: true))
        using (var source = new PdfDocument(new PdfReader(input)))
        {
            var pageCount = source.GetNumberOfPages();
            for (var page = 1; page <= pageCount; page++)
            {
                ct.ThrowIfCancellationRequested();

                using var pageBuffer = new MemoryStream();
                var writer = new PdfWriter(pageBuffer);
                writer.SetCloseStream(false);
                using (var target = new PdfDocument(writer))
                {
                    source.CopyPagesTo(page, page, target);
                }

                var entry = archive.CreateEntry($"{baseName}_page_{page}.pdf", CompressionLevel.Fastest);
                await using var entryStream = entry.Open();
                pageBuffer.Position = 0;
                await pageBuffer.CopyToAsync(entryStream, ct);
            }
        }

        return zipBuffer.ToArray();
    }
}

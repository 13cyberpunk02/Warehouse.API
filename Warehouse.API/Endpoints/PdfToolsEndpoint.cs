using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class PdfToolsEndpoint
{
    private const long MaxPdfSize = 100 * 1024 * 1024; // 100 МБ

    public static IEndpointRouteBuilder MapPdfToolsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/pdf-tools");
        group.MapPost("/split-pdf", SplitPdf)
            .RequireAuthorization()
            .DisableAntiforgery();
        return group;
    }

    private static async Task<IResult> SplitPdf(IFormFile file, IPdfProcessingService pdfService, CancellationToken ct)
    {
        if (file.Length == 0)
            return Results.BadRequest("Файл пустой");
        if (file.Length > MaxPdfSize)
            return Results.BadRequest("Файл больше 100 МБ");
        if (!string.Equals(Path.GetExtension(file.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
            return Results.BadRequest("Нужен PDF-файл");

        try
        {
            await using var stream = file.OpenReadStream();
            var zip = await pdfService.SplitPdfToZipAsync(stream, file.FileName, ct);
            var zipName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_pages.zip";
            return Results.File(zip, "application/zip", zipName);
        }
        catch (Exception ex) when (ex is iText.Kernel.Exceptions.PdfException or iText.IO.Exceptions.IOException)
        {
            return Results.BadRequest("Не удалось прочитать PDF: файл повреждён или защищён паролем");
        }
    }
}

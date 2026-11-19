using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Printer;
using Warehouse.API.Data.Models.DTO_s.Responses.Printer;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.PrinterErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Validators.PrinterValidators;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class PrinterService(DataContext context, SavePrinterRequestValidator validator) : IPrinterService
{
    public async Task<Result> GetAllPrinters()
    {
        var printers = await context.Printers
            .AsNoTracking()
            .OrderBy(p => p.Model)
            .Select(p => new PrinterResponse(
                p.Id,
                p.Model,
                p.Note,
                p.Cartridges
                    .OrderBy(c => c.Model)
                    .Select(c => new PrinterCartridgeResponse(c.Id, c.Model, c.Quantity))
                    .ToList()))
            .ToListAsync();
        return Result.Success(printers);
    }

    public async Task<Result> CreatePrinter(SavePrinterRequest request)
    {
        var check = await Validate(request, excludeId: null);
        if (check is not null) return check;

        var printer = new Printer
        {
            Model = request.Model.Trim(),
            Note = request.Note?.Trim() ?? string.Empty,
            Cartridges = await LoadCartridges(request.CartridgeIds),
        };
        context.Printers.Add(printer);
        await context.SaveChangesAsync();
        return Result.Success($"Принтер {printer.Model} добавлен");
    }

    public async Task<Result> UpdatePrinter(int id, SavePrinterRequest request)
    {
        var printer = await context.Printers.Include(p => p.Cartridges).FirstOrDefaultAsync(p => p.Id == id);
        if (printer is null)
            return Result.Failure(PrinterErrors.PrinterNotFound);

        var check = await Validate(request, excludeId: id);
        if (check is not null) return check;

        printer.Model = request.Model.Trim();
        printer.Note = request.Note?.Trim() ?? string.Empty;
        printer.Cartridges.Clear();
        printer.Cartridges.AddRange(await LoadCartridges(request.CartridgeIds));
        await context.SaveChangesAsync();
        return Result.Success($"Принтер {printer.Model} сохранён");
    }

    public async Task<Result> DeletePrinter(int id)
    {
        var printer = await context.Printers.FirstOrDefaultAsync(p => p.Id == id);
        if (printer is null)
            return Result.Failure(PrinterErrors.PrinterNotFound);

        // Связи с картриджами удаляются каскадно, сами картриджи остаются
        context.Printers.Remove(printer);
        await context.SaveChangesAsync();
        return Result.Success($"Принтер {printer.Model} удалён");
    }

    /// <summary>Валидация запроса, уникальность модели и существование картриджей. null — всё в порядке.</summary>
    private async Task<Result?> Validate(SavePrinterRequest request, int? excludeId)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(validation.Errors.Select(e => e.ErrorMessage)));

        var model = request.Model.Trim();
        var exists = await context.Printers.AnyAsync(p => p.Model.ToLower() == model.ToLower() && p.Id != excludeId);
        if (exists)
            return Result.Failure(PrinterErrors.PrinterExists);

        var ids = request.CartridgeIds?.Distinct().ToList() ?? [];
        if (ids.Count > 0 && await context.Cartridges.CountAsync(c => ids.Contains(c.Id)) != ids.Count)
            return Result.Failure(PrinterErrors.CartridgesNotFound);

        return null;
    }

    private async Task<List<Cartridge>> LoadCartridges(List<int>? ids)
    {
        var distinct = ids?.Distinct().ToList() ?? [];
        return distinct.Count == 0 ? [] : await context.Cartridges.Where(c => distinct.Contains(c.Id)).ToListAsync();
    }
}

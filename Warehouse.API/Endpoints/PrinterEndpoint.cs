using Warehouse.API.Data.Models.DTO_s.Requests.Printer;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class PrinterEndpoint
{
    public static IEndpointRouteBuilder MapPrinterEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/printer");
        group.MapGet("/get-all-printers", GetAll).RequireAuthorization("Cartridge");
        group.MapPost("/create-printer", Create).RequireAuthorization("Cartridge");
        group.MapPut("/update-printer/{printerId:int}", Update).RequireAuthorization("Cartridge");
        group.MapDelete("/delete-printer/{printerId:int}", Delete).RequireAuthorization("Cartridge");
        return group;
    }

    private static async Task<IResult> GetAll(IPrinterService service) =>
        (await service.GetAllPrinters()).ToHttpResponse();

    private static async Task<IResult> Create(IPrinterService service, SavePrinterRequest request) =>
        (await service.CreatePrinter(request)).ToHttpResponse();

    private static async Task<IResult> Update(IPrinterService service, int printerId, SavePrinterRequest request) =>
        (await service.UpdatePrinter(printerId, request)).ToHttpResponse();

    private static async Task<IResult> Delete(IPrinterService service, int printerId) =>
        (await service.DeletePrinter(printerId)).ToHttpResponse();
}

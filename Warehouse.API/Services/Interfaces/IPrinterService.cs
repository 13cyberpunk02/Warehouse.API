using Warehouse.API.Data.Models.DTO_s.Requests.Printer;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IPrinterService
{
    Task<Result> GetAllPrinters();
    Task<Result> CreatePrinter(SavePrinterRequest request);
    Task<Result> UpdatePrinter(int id, SavePrinterRequest request);
    Task<Result> DeletePrinter(int id);
}

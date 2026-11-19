using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IPdfFileManageService
{
    Task<Result> SplitPdfAsync(string filePath); 
}
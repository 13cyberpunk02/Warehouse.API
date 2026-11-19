namespace Warehouse.API.Services.Interfaces;

public interface IArchiveService
{
    Task<string> CreateArchiveAsync(IEnumerable<string> files, string baseDirectory);
}
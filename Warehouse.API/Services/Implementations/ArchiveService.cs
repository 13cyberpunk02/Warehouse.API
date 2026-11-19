using System.IO.Compression;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class ArchiveService : IArchiveService
{
    public async Task<string> CreateArchiveAsync(IEnumerable<string> files, string baseDirectory)
    {
        var archivePath = Path.Combine(baseDirectory, "split_pages.zip");

        using var archive = ZipFile.Open(archivePath, ZipArchiveMode.Create);
        foreach (var file in files)
        {
            archive.CreateEntryFromFile(file, Path.GetFileName(file));
        }

        return archivePath;
    }
}
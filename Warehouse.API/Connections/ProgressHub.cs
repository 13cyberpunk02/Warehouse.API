using Microsoft.AspNetCore.SignalR;

namespace Warehouse.API.Connections;

public class ProgressHub : Hub
{
    public async Task UploadProgress(string jobId)
    {
        await Clients.Group(jobId).SendAsync("UploadProgressUpdate");
    }

    public async Task ProcessingProgress(string jobId)
    {
        await Clients.Group(jobId).SendAsync("ProcessingProgressUpdate");
    }
}
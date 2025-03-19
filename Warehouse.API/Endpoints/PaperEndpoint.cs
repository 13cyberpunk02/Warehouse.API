using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Paper;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class PaperEndpoint
{
    public static IEndpointRouteBuilder MapPaperEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/paper");
        group.MapGet("/get-all-papers", GetAllPapers).RequireAuthorization();
        group.MapGet("/get-paper/{paperId:int}", GetPaperById).RequireAuthorization();
        group.MapPost("/add-paper", AddPaper).RequireAuthorization();
        group.MapPut("/update-paper", UpdatePaper).RequireAuthorization();
        group.MapDelete("/delete-paper/{paperId:int}", DeletePaper).RequireAuthorization();
        return group;   
    }

    private static async Task<IResult> GetAllPapers(IPaperService paperService)
    {
        var response = await paperService.GetAllPapers();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetPaperById(IPaperService paperService, int paperId)
    {
        var response = await paperService.GetPaperById(paperId);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> AddPaper(IPaperService paperService, AddPaperRequest request)
    {
        var response = await paperService.CreatePaper(request);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> UpdatePaper(IPaperService paperService, Paper request)
    {
        var response = await paperService.UpdatePaper(request);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> DeletePaper(IPaperService paperService, int paperId)
    {
        var response = await paperService.DeletePaper(paperId);
        return response.ToHttpResponse();
    }
}
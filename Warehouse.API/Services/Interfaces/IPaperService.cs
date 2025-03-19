using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Paper;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IPaperService
{
    Task<Result> GetAllPapers();
    Task<Result> GetPaperById(int id);
    Task<Result> CreatePaper(AddPaperRequest paper);
    Task<Result> UpdatePaper(Paper paper);
    Task<Result> DeletePaper(int id);
}
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Paper;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.PaperErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Validators.PaperValidator;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class PaperService(
    DataContext context, 
    AddPaperRequestValidator addPaperRequestValidator) : IPaperService
{
    private readonly DataContext _context = context;
    public async Task<Result> GetAllPapers()
    {
        var papers = await _context.Papers.ToListAsync();
        if (papers.Count == 0)
            return Result.Failure(PaperErrors.PapersNotFound);
        return Result.Success(papers);
    }

    public async Task<Result> GetPaperById(int id)
    {
        var paper = await _context.Papers.FirstOrDefaultAsync(x => x.Id == id);
        if (paper is null)
            return Result.Failure(PaperErrors.PaperNotFound);
        return Result.Success(paper);
    }

    public async Task<Result> CreatePaper(AddPaperRequest paper)
    {
        var modelValidator = await addPaperRequestValidator.ValidateAsync(paper);
        if(!modelValidator.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidator.Errors.Select(x => x.ErrorMessage)));
        
        var isPaperExist = await _context.Papers.AnyAsync(x => x.Format == paper.Format);
        if(isPaperExist)
            return Result.Failure(PaperErrors.PaperExists);

        var paperToAdd = new Paper
        {
            Format = paper.Format,
            Quantity = paper.Quantity
        };
        
        await _context.Papers.AddAsync(paperToAdd);
        var result = await _context.SaveChangesAsync();
        if(result == 0)
            return Result.Failure(PaperErrors.PaperDidntSave);
        return Result.Success("Бумага успешно добавлена");
    }

    public async Task<Result> UpdatePaper(Paper paper)
    {
        var paperToUpdate = await _context.Papers.FirstOrDefaultAsync(x => x.Id == paper.Id);
        if(paperToUpdate is null)
            return Result.Failure(PaperErrors.PaperNotFound);
        paperToUpdate.Format = paper.Format;
        paperToUpdate.Quantity = paper.Quantity;
        
        _context.Papers.Update(paperToUpdate);
        var result = await _context.SaveChangesAsync();
        if(result == 0)
            return Result.Failure(PaperErrors.PaperDidntSave);
        return Result.Success("Бумага успешно отредактирована");
    }

    public async Task<Result> DeletePaper(int id)
    {
        var paperToDelete = await _context.Papers.FirstOrDefaultAsync(x => x.Id == id);
        if(paperToDelete is null)
            return Result.Failure(PaperErrors.PaperNotFound);
        _context.Papers.Remove(paperToDelete);
        var result = await _context.SaveChangesAsync();
        if(result == 0)
            return Result.Failure(PaperErrors.PaperDidntSave);
        return Result.Success("Бумага успешно удалена");
    }    
}
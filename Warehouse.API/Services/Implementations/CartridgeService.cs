using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Cartridge;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.CartridgeErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Validators.CartridgeValidator;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class CartridgeService(DataContext context,
    UpdateCartridgeRequestValidator updateCartridgeRequestValidator,
    AddCartridgeRequestValidator cartridgeRequestValidator) : ICartridgeService
{
    public async Task<Result> GetAllCartridges()
    {
        var cartridges = await context.Cartridges.ToListAsync();
        return Result.Success(cartridges);
    }

    public async Task<Result> GetCartridgeById(int id)
    {
        var cartridge = await context.Cartridges.FirstOrDefaultAsync(x => x.Id == id);
        if(cartridge is null)
            return Result.Failure(CartridgeErrors.CartridgeNotFound);
        
        return Result.Success(cartridge);
    }

    public async Task<Result> CreateCartridge(AddCartridgeRequest request)
    {
        var modelValidation = await cartridgeRequestValidator.ValidateAsync(request);
        if(!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));

        var cartridgeExists = context.Cartridges.Any(x => x.Model == request.Model);
        if (cartridgeExists)
            return Result.Failure(CartridgeErrors.CartridgeExists);

        await context.Cartridges.AddAsync(new Cartridge
        {
            Model = request.Model,
            Quantity = request.Quantity
        });
        
        var result = await context.SaveChangesAsync();
        if(result < 1)
            return Result.Failure(CartridgeErrors.CartridgeDidntSave);
        return Result.Success("Картридж добавлен успешно");
    }

    public async Task<Result> UpdateCartridge(UpdateCartridgeRequest request)
    {
        var modelValidation = await updateCartridgeRequestValidator.ValidateAsync(request);
        if(!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));
        var cartridge = await context.Cartridges.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(cartridge is null)
            return Result.Failure(CartridgeErrors.CartridgeExists);
        
        cartridge.Model = request.Model;
        cartridge.Quantity = request.Quantity;        
        context.Cartridges.Update(cartridge);
        
        var result = await context.SaveChangesAsync();
        if(result < 1)
            return Result.Failure(CartridgeErrors.CartridgeDidntSave);
        return Result.Success("Картридж успешно обновлен");
    }

    public async Task<Result> DeleteCartridge(int id)
    {
        var cartridge = await context.Cartridges.FirstOrDefaultAsync(x => x.Id == id);
        if(cartridge is null)
            return Result.Failure(CartridgeErrors.CartridgeNotFound);
        
        context.Cartridges.Remove(cartridge);
        var result = await context.SaveChangesAsync();
        if(result < 1)
            return Result.Failure(CartridgeErrors.CartridgeDidntSave);
        return Result.Success("Картридж умпешно удален");
    }
}
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Delivery;
using Warehouse.API.Data.Models.DTO_s.Responses.Delivery;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.AccountErrors;
using Warehouse.API.Data.Models.Error.ErrorTypes.DeliveryErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Validators.DeliveryValidators;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class DeliveryService(DataContext context,
    AddDeliveryRequestValidator requestValidator) : IDeliveryService
{
    public async Task<Result> GetAllPaperDeliveries()
    {
        var deliveries = await context.Deliveries
            .Include(d => d.DeliveryItem)
            .Where(d => d.DeliveryItem!.ItemType == "paper")
            .OrderByDescending(d => d.DeliveredAt)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        var response = deliveries.Select(x => new DeliveriesResponse(
            Id: x.Id,
            DeliveredAt: x.DeliveredAt,
            DeliveredBy: x.DeliveredUser,
            ReceivedDepartment: x.ReceivedUserDepartment,
            ReceivedUser: x.ReceivedUser,
            ItemName: x.DeliveryItem.Name,
            Quantity: x.DeliveryItem.Quantity
            ));

        return Result.Success(response);
    }

    public async Task<Result> GetAllCartridgeDeliveries()
    {
        var deliveries = await context.Deliveries
            .Include(d => d.DeliveryItem)
            .Where(d => d.DeliveryItem!.ItemType == "cartridge")
            .OrderByDescending(d => d.DeliveredAt)
            .ToListAsync();

        var response = deliveries.Select(x => new DeliveriesResponse(
            Id: x.Id,
            DeliveredAt: x.DeliveredAt,
            DeliveredBy: x.DeliveredUser,
            ReceivedDepartment: x.ReceivedUserDepartment,
            ReceivedUser: x.ReceivedUser,
            ItemName: x.DeliveryItem.Name,
            Quantity: x.DeliveryItem.Quantity
        ));

        return Result.Success(response);
    }

    public async Task<Result> AddPaperDelivery(AddDeliveryRequest request, string deliveredUserId)
    {
        var modelValidation = await requestValidator.ValidateAsync(request);
        if(!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));

        if(string.IsNullOrEmpty(deliveredUserId))
            return Result.Failure(DeliveryErrors.AuthorizedUserTokenNotProvided);

        var deliveredUser = await context.Users.FirstOrDefaultAsync(user => user.Id == deliveredUserId);
        if (deliveredUser is null)
            return Result.Failure(AccountErrors.UserNotFound);
        var receivedUser = await context.DeliveryUsers
            .Include(u => u.Department)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == request.ReceivedUserId);
        if (receivedUser is null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        var paper = await context.Papers.FirstOrDefaultAsync(x => x.Id == request.ItemId);
        if (paper is null)
            return Result.Failure(DeliveryErrors.PaperIsNotFound);
        if (paper.Quantity < request.Quantity)
            return Result.Failure(DeliveryErrors.DeliveryPaperQuantityIsGreater);

        var deliveryItem = new DeliveryItem()
        {
            Name = paper.Format,
            Quantity = request.Quantity,
            ItemType = "paper"
        };
        
        // Позиция, выдача и списание остатка сохраняются одним SaveChanges (одна транзакция)
        var deliveryToAdd = new Delivery()
        {
            ReceivedUser = receivedUser.Fullname,
            ReceivedUserDepartment = receivedUser.Department.Name,
            DeliveredAt = DateTime.Now,
            DeliveredUser = deliveredUser.Fullname,
            DeliveryItem = deliveryItem
        };
        
        paper.Quantity -= request.Quantity;
        context.Papers.Update(paper);
        await context.Deliveries.AddAsync(deliveryToAdd);
        var result = await context.SaveChangesAsync();
        
        if(result == 0)
            return Result.Failure(DeliveryErrors.DeliverySaveError);
        
        return Result.Success("Выдача успешно добавлена");        
    }

    public async Task<Result> AddCartridgeDelivery(AddDeliveryRequest request, string deliveredUserId)
    {
        var modelValidation = await requestValidator.ValidateAsync(request);
        if(!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));

        if(string.IsNullOrEmpty(deliveredUserId))
            return Result.Failure(DeliveryErrors.AuthorizedUserTokenNotProvided);

        var deliveredUser = await context.Users.FirstOrDefaultAsync(user => user.Id == deliveredUserId);
        if (deliveredUser is null)
            return Result.Failure(AccountErrors.UserNotFound);
        var receivedUser = await context.DeliveryUsers
            .Include(u => u.Department)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == request.ReceivedUserId);
        if (receivedUser is null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        var cartridge = await context.Cartridges.FirstOrDefaultAsync(x => x.Id == request.ItemId);
        if(cartridge is null)
            return Result.Failure(DeliveryErrors.CartridgeIsNotFound);
        
        if (cartridge.Quantity < request.Quantity)
            return Result.Failure(DeliveryErrors.DeliveryCartridgeQuantityIsGreater);
        
        var deliveryItem = new DeliveryItem()
        {
            Name = cartridge.Model,
            Quantity = request.Quantity,
            ItemType = "cartridge"
        };
        
        // Позиция, выдача и списание остатка сохраняются одним SaveChanges (одна транзакция)
        var deliveryToAdd = new Delivery()
        {
            ReceivedUser = receivedUser.Fullname,
            ReceivedUserDepartment = receivedUser.Department.Name,
            DeliveredAt = DateTime.Now,
            DeliveredUser = deliveredUser.Fullname,
            DeliveryItem = deliveryItem
        };
        
        cartridge.Quantity -= request.Quantity;
        context.Cartridges.Update(cartridge);
        await context.Deliveries.AddAsync(deliveryToAdd);
        var result = await context.SaveChangesAsync();
        
        if(result == 0)
            return Result.Failure(DeliveryErrors.DeliverySaveError);
        
        return Result.Success("Выдача успешно добавлена");
    }
}
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
    IJwtService jwtService,
    AddDeliveryRequestValidator requestValidator) : IDeliveryService
{
    public async Task<Result> GetAllDeliveries()
    {
        var deliveries = await context.Deliveries
            .ToListAsync();
        if(!deliveries.Any())
            return Result.Failure(DeliveryErrors.DeliveryNotFound);

        var response = deliveries.Select(x => new DeliveriesResponse(
            Id: x.Id,
            DeliveredAt: x.DeliveredAt,
            DeliveredBy: x.DeliveredUser,
            ReceivedDepartment: x.ReceivedUserDepartment,
            ReceivedUser: x.ReceivedUser,
            PaperFormat: x.PaperFormat,
            Quantity: x.Quantity
            ));

        return Result.Success(response);
    }
    public async Task<Result> AddDelivery(AddDeliveryRequest request)
    {
        var modelValidation = await requestValidator.ValidateAsync(request);
        if(!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));

        var principal = jwtService.GetPrincipalFromExpiredToken(request.DeliveredUserToken);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId is null)
            return Result.Failure(DeliveryErrors.AuthorizedUserTokenNotProvided);
        
        var deliveredUser = await context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        if (deliveredUser is null)
            return Result.Failure(AccountErrors.UserNotFound);
        var receivedUser = await context.Users
            .Include(u => u.Department)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == request.ReceivedUserId);
        if (receivedUser is null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        var paper = await context.Papers.FirstOrDefaultAsync(x => x.Id == request.PaperId);
        if (paper is null)
            return Result.Failure(DeliveryErrors.PaperIsNotFound);
        if (paper.Quantity < request.Quantity)
            return Result.Failure(DeliveryErrors.DeliveryPaperQuantityIsGreater);
        
        var deliveryToAdd = new Delivery()
        {
            ReceivedUser = receivedUser.Fullname,
            ReceivedUserDepartment = receivedUser.Department.Name,
            DeliveredAt = DateTime.Now,
            DeliveredUser = deliveredUser.Fullname,
            PaperFormat = request.PaperFormat,
            Quantity = request.Quantity
        };
        
        paper.Quantity -= request.Quantity;
        context.Papers.Update(paper);
        await context.Deliveries.AddAsync(deliveryToAdd);
        var result = await context.SaveChangesAsync();
        
        if(result == 0)
            return Result.Failure(DeliveryErrors.DeliverySaveError);
        
        return Result.Success("Выдача успешно добавлена");        
    }
}
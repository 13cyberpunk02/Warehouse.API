using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Delivery;
using Warehouse.API.Data.Models.DTO_s.Responses.Delivery;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.DeliveryErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Validators.DeliveryValidators;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class DeliveryService(DataContext context, 
    AddDeliveryRequestValidator requestValidator) : IDeliveryService
{
    public async Task<Result> GetAllDeliveries()
    {
        var deliveries = await context.Deliveries
            .Include(u => u.DeliveredUser)
            .Include(u => u.ReceivedUser).ThenInclude(appUser => appUser.Department)
            .Include(u => u.Paper)
            .OrderBy(x => x.DeliveredAt)
            .ToListAsync();
        if(!deliveries.Any())
            return Result.Failure(DeliveryErrors.DeliveryNotFound);

        var response = deliveries.Select(x => new DeliveriesResponse(
            Id: x.Id,
            DeliveredAt: x.DeliveredAt,
            DeliveredBy: x.DeliveredUser.Fullname,
            ReceivedDepartment: x.ReceivedUser.Department.Name,
            ReceivedUser: x.ReceivedUser.Fullname,
            Paper: x.Paper.Format,
            Quantity: x.Quantity
            ));

        return Result.Success(response);
    }

    public async Task<Result> GetAllDeliveriesByDepartmentOrUser(GetDeliveryByUserOrDepartment request)
    {
        if (request.UserId == null)
            return Result.Failure(DeliveryErrors.UserIdAndDepartmentIdNotProvided);
        
        var deliveries = await context.Deliveries
            .Include(u => u.DeliveredUser)
            .Include(u => u.ReceivedUser)
            .ThenInclude(appUser => appUser.Department)
            .Include(u => u.Paper)
            .Where(x => 
                x.ReceivedUser.Id == request.UserId 
                && x.DeliveredAt > request.StartDate 
                && x.DeliveredAt < request.EndDate)
            .OrderBy(x => x.DeliveredAt)
            .ToListAsync();

        var result = deliveries.Select(x => new DeliveriesResponse(
                Id: x.Id,
                DeliveredAt: x.DeliveredAt,
                DeliveredBy: x.DeliveredUser.Fullname,
                ReceivedDepartment: x.ReceivedUser.Department.Name,
                ReceivedUser: x.ReceivedUser.Fullname,
                Paper: x.Paper.Format,
                Quantity: x.Quantity
            ))
            .Where(x => x.ReceivedUser == request.UserId).ToList();
        return Result.Success(result);
    }

    public async Task<Result> AddDelivery(AddDeliveryRequest request)
    {
        var modelValidation = await requestValidator.ValidateAsync(request);
        if(!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));

        var deliveryToAdd = new Delivery()
        {
            ReceivedUserId = request.ReceivedUserId,
            DeliveredAt = DateTime.Now,
            DeliveredUserId = request.DeliveredUserId,
            PaperId = request.PaperId,
            Quantity = request.Quantity
        };

        var paper = await context.Papers.FirstOrDefaultAsync(x => x.Id == request.PaperId);
        if (paper is null)
            return Result.Failure(DeliveryErrors.PaperIsNotFound);
        if (paper.Quantity < request.Quantity)
            return Result.Failure(DeliveryErrors.DeliveryPaperQuantityIsGreater);
        
        paper.Quantity -= request.Quantity;
        context.Papers.Update(paper);
        await context.Deliveries.AddAsync(deliveryToAdd);
        var result = await context.SaveChangesAsync();
        
        if(result == 0)
            return Result.Failure(DeliveryErrors.DeliverySaveError);
        
        return Result.Success("Выдача успешно добавлена");        
    }
}
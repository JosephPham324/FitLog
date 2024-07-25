using FitLog.Application.Common.Models;
using FitLog.Application.Equipments.Commands.CreateEquipment;
using FitLog.Application.Equipments.Commands.Delete;
using FitLog.Application.Equipments.Commands.UpdateEquipment;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Application.Equipments.Queries.GetEquipmentDetails;
using FitLog.Application.TodoItems.Commands.UpdateTodoItemDetail;
using Microsoft.AspNetCore.Mvc;
using FitLog.Application.Common.Interfaces;
using FitLog.Web.Services;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class Equipments : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public Equipments()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)

            .MapGet(GetEquipmentsWithPagination, "get-all")
            .MapGet(GetEquipmentById, "{id}")
            .MapPost(CreateEquipment)
            .MapPut(UpdateEquipment, "{id}")
            .MapDelete(DeleteEquipment, "{id}");
    }

    public Task<PaginatedList<EquipmentDTO>> GetEquipmentsWithPagination(ISender sender, [AsParameters] GetEquipmentsWithPaginationQuery query)
    {
        return sender.Send(query);
    }


    public async Task<EquipmentDetailsDTO> GetEquipmentById(ISender sender, int id)
    {
        var query = new GetEquipmentDetailsQuery { EquipmentId = id };
        var result = await sender.Send(query);
        return result;
    }

    [Microsoft.AspNetCore.Authorization.Authorize("AdminOnly")]
    public Task<Result> CreateEquipment(ISender sender, [FromBody] CreateEquipmentCommand command)
    {
        return sender.Send(command);
    }

    [Microsoft.AspNetCore.Authorization.Authorize("AdminOnly")]
    public async Task<Result> UpdateEquipment(ISender sender, int id, [FromBody] UpdateEquipmentCommand command)
    {
        if (id != command.EquipmentId) return Result.Failure(["Id doesn't match instance"]);
        var result = await sender.Send(command);
        return result;
    }

    [Microsoft.AspNetCore.Authorization.Authorize("AdminOnly")]
    public async Task<Result> DeleteEquipment(ISender sender, int id, [FromBody] DeleteEquipmentCommand command)
    {
        if (id != command.EquipmentId) return Result.Failure(["Id doesn't match instance"]);
        var result = await sender.Send(command);
        return result;
    }
}

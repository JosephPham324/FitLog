using FitLog.Application.Common.Models;
using FitLog.Application.Equipments.Commands.CreateEquipment;
using FitLog.Application.Equipments.Commands.Delete;
using FitLog.Application.Equipments.Commands.UpdateEquipment;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Application.Equipments.Queries.GetEquipmentDetails;
using FitLog.Application.TodoItems.Commands.UpdateTodoItemDetail;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class Equipments : EndpointGroupBase
{
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

    public async Task<object> GetEquipmentById(ISender sender, int id)
    {
        var query = new GetEquipmentDetailsQuery { EquipmentId = id };
        var result = await sender.Send(query);
        return result;
    }

    public Task<int> CreateEquipment(ISender sender, [AsParameters] CreateEquipmentCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateEquipment(ISender sender, int id, [FromBody] UpdateEquipmentCommand command)
    {
        if (id != command.EquipmentId) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteEquipment(ISender sender, int id, [FromBody] DeleteEquipmentCommand command)
    {
        if (id != command.EquipmentId) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
}

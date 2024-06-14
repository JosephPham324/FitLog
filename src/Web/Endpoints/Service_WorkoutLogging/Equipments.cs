using FitLog.Application.Common.Models;
using FitLog.Application.Equipments.Commands.CreateEquipment;
using FitLog.Application.Equipments.Commands.Delete;
using FitLog.Application.Equipments.Commands.UpdateEquipment;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Application.Equipments.Queries.GetEquipmentDetails;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class Equipments : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetEquipmentsWithPagination, "get-all")
            .MapGet(GetEquipmentById, "{id}")
            .MapPost(CreateEquipment)
            .MapPut(DeleteEquipment, "{id}")
            .MapDelete(UpdateEquipment, "{id}");
    }

    public Task<PaginatedList<EquipmentDTO>> GetEquipmentsWithPagination(ISender sender, [AsParameters] GetEquipmentsWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public async Task<object> GetEquipmentById(ISender sender, [AsParameters] GetEquipmentDetailsQuery query)
    {
        var result = await sender.Send(query);
        return result;
    }

    public Task<int> CreateEquipment(ISender sender, [AsParameters] CreateEquipmentCommand command)
    {
        return sender.Send(command);
    }

    public Task<bool> UpdateEquipment(ISender sender, [AsParameters] UpdateEquipmentCommand command)
    {
        return sender.Send(command);
    }

    public Task<bool> DeleteEquipment(ISender sender, [AsParameters] DeleteEquipmentCommand command)
    {
        return sender.Send(command);
    }
}

using FitLog.Application.Common.Models;
using FitLog.Application.MuscleGroups.Commands.CreateMuscleGroup;
using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination;
using FitLog.Application.TrainingSurveys.Commands;

namespace FitLog.Web.Endpoints;

public class MuscleGroups : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateMuscleGroup, "create")
            .MapGet(GetMuscleGroupsList, "get-list");
    }
    public Task<int> CreateMuscleGroup(ISender sender, [AsParameters] CreateMuscleGroupCommand command)
    {
        return sender.Send(command);
    }

    public Task<PaginatedList<MuscleGroupDTO>> GetMuscleGroupsList(ISender sender, [AsParameters] GetMuscleGroupsListWithPaginationQuery query)
    {
        return sender.Send(query);
    }
}

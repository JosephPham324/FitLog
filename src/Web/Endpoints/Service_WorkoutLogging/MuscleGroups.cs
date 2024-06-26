using FitLog.Application.Common.Models;
using FitLog.Application.Exercises.Commands.CreateExercise;
using FitLog.Application.Exercises.Commands.DeleteExercise;
using FitLog.Application.Exercises.Commands.UpdateExercise;
using FitLog.Application.Exercises.Queries.GetExerciseDetails;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Application.MuscleGroups.Commands.CreateMuscleGroup;
using FitLog.Application.MuscleGroups.Commands.DeleteMuscleGroup;
using FitLog.Application.MuscleGroups.Commands.UpdateMuscleGroup;
using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupDetails;
using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination;
using FitLog.Application.TrainingSurveys.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class MuscleGroups : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateMuscleGroup, "create")
            .MapGet(GetMuscleGroupsList, "get-list")
            .MapGet(GetMuscleGroupsById, "{id}")
            .MapPut(UpdateMuscleGroup, "{id}")
            .MapDelete(DeleteMuscleGroup, "{id}");
    }
    public Task<int> CreateMuscleGroup(ISender sender, [AsParameters] CreateMuscleGroupCommand command)
    {
        return sender.Send(command);
    }

    public Task<PaginatedList<Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination.MuscleGroupDTO>> GetMuscleGroupsList(ISender sender, [AsParameters] GetMuscleGroupsListWithPaginationQuery query)
    {
        return sender.Send(query);
    }


    public Task<Application.MuscleGroups.Queries.GetMuscleGroupDetails.MuscleGroupDTO> GetMuscleGroupsById(ISender sender, [AsParameters] GetMuscleGroupDetailsQuery query)
    {
        return sender.Send(query);

    }

    public Task<UpdateMuscleGroupDTO> UpdateMuscleGroup(ISender sender, [AsParameters] UpdateMuscleGroupCommand command)
    {
        return sender.Send(command);
    }

    public Task<bool> DeleteMuscleGroup(ISender sender, [AsParameters] DeleteMuscleGroupCommand command)
    {
        return sender.Send(command);
    }
}

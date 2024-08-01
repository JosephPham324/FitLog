using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Common.Security;
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
using FitLog.Application.MuscleGroups.Queries.PaginatedSearchMuscleGroup;
using FitLog.Application.TrainingSurveys.Commands;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class MuscleGroups : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public MuscleGroups()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }
    public override void Map(WebApplication app)
    {
        //Get data
        app.MapGroup(this)
            //All users can view data
            .MapGet(GetMuscleGroupsList, "get-list")
            .MapGet(GetMuscleGroupsById, "{id}");
        //Changes data
        app.MapGroup(this)
            //Only admin can change data
            .RequireAuthorization("AdminOnly")
            .MapPost(CreateMuscleGroup, "create")
            .MapPut(UpdateMuscleGroup, "{id}")
            .MapDelete(DeleteMuscleGroup, "{id}");
    }

    public Task<Result> CreateMuscleGroup(ISender sender, [FromBody] CreateMuscleGroupCommand command)
    {
        return sender.Send(command);
    }

    public Task<PaginatedList<Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination.MuscleGroupDTO>> GetMuscleGroupsList(ISender sender, [AsParameters] GetMuscleGroupsListWithPaginationQuery query)
    {
        return sender.Send(query);
    }


    public Task<PaginatedList<Application.MuscleGroups.Queries.GetMuscleGroupDetails.MuscleGroupDTO>> SearchMuscleGroup(ISender sender, [AsParameters] PaginatedSearchMuscleGroupQuery query)
    {
        return sender.Send(query);
    }

    public Task<Application.MuscleGroups.Queries.GetMuscleGroupDetails.MuscleGroupDTO> GetMuscleGroupsById(ISender sender, [AsParameters] GetMuscleGroupDetailsQuery query)
    {
        return sender.Send(query);
    }

    public Task<Result> UpdateMuscleGroup(ISender sender, int id, [FromBody] UpdateMuscleGroupCommand command)
    {
        return sender.Send(command);
    }
    public Task<Result> DeleteMuscleGroup(ISender sender, [FromBody] DeleteMuscleGroupCommand command)
    {
        return sender.Send(command);
    }
}

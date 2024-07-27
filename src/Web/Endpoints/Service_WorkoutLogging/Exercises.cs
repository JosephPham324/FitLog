using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Exercises.Commands.CreateExercise;
using FitLog.Application.Exercises.Commands.DeleteExercise;
using FitLog.Application.Exercises.Commands.ImportExercises;
using FitLog.Application.Exercises.Commands.UpdateExercise;
using FitLog.Application.Exercises.Queries.GetExerciseDetails;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Application.Exercises.Queries.GetExercsieTypes;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;


public class Exercises : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public Exercises()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }

    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetExercisesWithPagination, "paginated-all")
            .MapGet(GetExerciseTypes, "exercise-types")
            .MapGet(GetExerciseById, "{id}");

        app.MapGroup(this)
            .RequireAuthorization("AdminOnly")
            .RequireAuthorization("CoachOnly")
            .MapPost(CreateExercise)
            .MapPost(ImportExercises, "import-exercises")
            .MapPut(UpdateExercise, "{id}")
            .MapDelete(DeleteExercise, "{id}");
    }

    public Task<PaginatedList<ExerciseDTO>> GetExercisesWithPagination(ISender sender, [AsParameters] GetExercisesWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public async Task<IResult> GetExerciseById(ISender sender, int id)
    {
        var result = await sender.Send(new GetExerciseByIdQuery { Id = id });
        return result is not null ? Results.Ok(result) : Results.NotFound();
    }

    public Task<Result> CreateExercise(ISender sender, [FromBody]CreateExerciseCommand command)
    {
        return sender.Send(command);
    }

    public Task<Result> UpdateExercise(ISender sender, int id, [FromBody] UpdateExerciseCommand command)
    {
        return sender.Send(command);
    }

    public Task<Result> DeleteExercise(ISender sender, [FromBody] DeleteExerciseCommand command)
    {
        return sender.Send(command);
    }

    public Task<IEnumerable<string?>> GetExerciseTypes(ISender sender, [AsParameters] GetExercsieTypesQuery query)
    {
        return sender.Send(query);
    }
    [Microsoft.AspNetCore.Authorization.Authorize("AdminOnly")]
    public Task<Result> ImportExercises(ISender sender, [FromBody] ImportExercisesCommand command)
    {
        return sender.Send(command);
    }
}


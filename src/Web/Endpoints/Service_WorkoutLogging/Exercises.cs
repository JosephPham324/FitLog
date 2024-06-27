using FitLog.Application.Common.Models;
using FitLog.Application.Exercises.Commands.CreateExercise;
using FitLog.Application.Exercises.Commands.DeleteExercise;
using FitLog.Application.Exercises.Commands.ImportExercises;
using FitLog.Application.Exercises.Commands.UpdateExercise;
using FitLog.Application.Exercises.Queries.GetExerciseDetails;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Application.Exercises.Queries.GetExercsieTypes;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;


public class Exercises : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetExercisesWithPagination, "paginated-all")
            .MapGet(GetExerciseTypes, "exercise-types")
            .MapGet(GetExerciseById, "{id}")
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

    public Task<int> CreateExercise(ISender sender, [FromBody]CreateExerciseCommand command)
    {
        return sender.Send(command);
    }

    public Task<bool> UpdateExercise(ISender sender, [FromBody] UpdateExerciseCommand command)
    {
        return sender.Send(command);
    }

    public Task<bool> DeleteExercise(ISender sender, [FromBody] DeleteExerciseCommand command)
    {
        return sender.Send(command);
    }

    public Task<IEnumerable<string?>> GetExerciseTypes(ISender sender, [AsParameters] GetExercsieTypesQuery query)
    {
        return sender.Send(query);
    }

    public Task<int> ImportExercises(ISender sender, [FromBody] ImportExercisesCommand command)
    {
        return sender.Send(command);
    }
}


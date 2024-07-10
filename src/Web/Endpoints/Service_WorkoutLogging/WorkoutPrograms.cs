using FitLog.Application.Common.Models;
using FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;
using FitLog.Application.WorkoutPrograms.Commands.CreateWorkoutProgram;
using FitLog.Application.WorkoutPrograms.Commands.DeleteWorkoutProgram;
using FitLog.Application.WorkoutPrograms.Commands.UpdateWorkoutProgram;
using FitLog.Application.WorkoutPrograms.Queries.GetWorkoutProgramDetails;
using FitLog.Application.WorkoutPrograms.Queries.GetWorkoutProgramsList;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class WorkoutPrograms : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetWorkoutProgramsList)
            .MapPost(CreateWorkoutProgram)
            .MapPut(UpdateWorkoutProgram, "{id}")
            .MapDelete(DeleteWorkoutProgram, "{id}")
            .MapGet(GetWorkoutProgramDetails, "Details/{id}");
    }

    public Task<List<WorkoutProgramListDTO>> GetWorkoutProgramsList(ISender sender, [AsParameters] GetWorkoutProgramsListQuery query)
    {
        return sender.Send(query);
    }

    public Task<Result> CreateWorkoutProgram(ISender sender, CreateWorkoutProgramCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateWorkoutProgram(ISender sender, int id, UpdateWorkoutProgramCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        var result = await sender.Send(command);
        return result.Success ? Results.NoContent() : Results.BadRequest(result.Errors);
    }

    public async Task<IResult> DeleteWorkoutProgram(ISender sender, int id)
    {
        var result = await sender.Send(new DeleteWorkoutProgramCommand { Id = id });
        return result.Success ? Results.NoContent() : Results.BadRequest(result.Errors);
    }

    public async Task<IResult> GetWorkoutProgramDetails(ISender sender, int id)
    {
        var result = await sender.Send(new GetWorkoutProgramDetailsQuery { Id = id });
        return result != null ? Results.Ok(result) : Results.NotFound();
    }
}

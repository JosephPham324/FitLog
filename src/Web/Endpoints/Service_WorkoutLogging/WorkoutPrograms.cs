using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;
using FitLog.Application.WorkoutPrograms.Commands.CreateWorkoutProgram;
using FitLog.Application.WorkoutPrograms.Commands.DeleteWorkoutProgram;
using FitLog.Application.WorkoutPrograms.Commands.EnrollProgram;
using FitLog.Application.WorkoutPrograms.Commands.UpdateWorkoutProgram;
using FitLog.Application.WorkoutPrograms.Queries.GetEnrollmentByUser;
using FitLog.Application.WorkoutPrograms.Queries.GetWorkoutProgramDetails;
using FitLog.Application.WorkoutPrograms.Queries.GetWorkoutProgramsList;
using FitLog.Web.Services;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;


namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class WorkoutPrograms : EndpointGroupBase
{
    private readonly IUserTokenService? _tokenService;
    private readonly IUser _identityService;

    public WorkoutPrograms()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }

    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetWorkoutProgramsList)
            .MapPost(CreateWorkoutProgram)
            .MapPut(UpdateWorkoutProgram, "{id}")
            .MapDelete(DeleteWorkoutProgram, "{id}")
            .MapGet(GetWorkoutProgramDetails, "details/{id}")
            .MapGet(GetEnrollmentsByUser, "enrollments/user/")
            .MapPost(EnrollProgram,"enroll/{id}");
    }

    public Task<List<WorkoutProgramListDTO>> GetWorkoutProgramsList(ISender sender, [AsParameters] GetWorkoutProgramsListQuery query)
    {
        return sender.Send(query);
    }

    public Task<Result> CreateWorkoutProgram(ISender sender, CreateWorkoutProgramCommand command)
    {
        command.UserId = _identityService.Id;
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
    public async Task<Result> EnrollProgram(ISender sender, int id, [FromBody] EnrollProgramCommand command)
    {

        command.UserId = _identityService.Id ?? "";
        if (id != command.ProgramId)
        {
            throw new BadHttpRequestException("Program id in the route does not match the program id in the request body");
        }
        return await sender.Send(command);
    }
    public async Task<List<ProgramEnrollmentDTO>> GetEnrollmentsByUser(ISender sender)
    {
        var userId = _tokenService?.GetUserIdFromToken();
        
        if (userId == null)  {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        var query = new GetEnrollmentsByUserQuery { UserId = userId };
        return await sender.Send(query);
    }
}

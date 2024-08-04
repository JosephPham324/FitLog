using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.MuscleGroups.Commands.DeleteMuscleGroup;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;
using FitLog.Application.WorkoutTemplates.Commands.CreatePersonalTemplate;
using FitLog.Application.WorkoutTemplates.Commands.CreateWorkoutTemplate;
using FitLog.Application.WorkoutTemplates.Commands.DeleteWorkoutTemplate;
using FitLog.Application.WorkoutTemplates.Commands.UpdateWorkoutTemplate;
using FitLog.Application.WorkoutTemplates.Queries.FilterWorkoutTemplates;
using FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;
using FitLog.Application.WorkoutTemplates.Queries.GetPublicTemplates;
using FitLog.Application.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;
using FitLog.Application.WorkoutTemplates.Queries.SearchWorkoutTemplateByName;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class WorkoutTemplates : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public WorkoutTemplates()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }

    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreatePersonalTemplate, "create-personal-template")
            .MapPost(CreateWorkoutTemplate, "create-workout-template")
            .MapPut(UpdateWorkoutTemplate, "update-workout-template/{id}")
            .MapDelete(DeleteWorkoutTemplate, "delete-workout-template/{id}")
            .MapGet(GetPublicTemplates, "get-public-templates")
            .MapGet(GetWorkoutTemplateDetails, "get-workout-template-details/{id}")
            .MapGet(FilterWorkoutTemplates, "filter-workout-templates");
    }

    public async Task<Result> CreatePersonalTemplate(ISender sender, [FromBody] CreatePersonalTemplateCommand command)
    {
        command.UserId = _identityService.Id ?? "";
        Result? result = null;
        try
        {
            result = await sender.Send(command);
        }
        catch (Exception e)
        {
            return Result.Failure([e.Message]);
        }
        return result ?? Result.Failure(["Failed to create template"]);
    }

    public async Task<Result> CreateWorkoutTemplate(ISender sender, [FromBody] CreateWorkoutTemplateCommand command)
    {
        command.UserId = _identityService.Id ?? "";
        Result? result = null;
        try
        {
            result = await sender.Send(command);
        } catch(Exception e)
        {
            return Result.Failure([e.Message]);
        }
        return result ?? Result.Failure(["Failed to create template"]);
    }

    public async Task<Result> UpdateWorkoutTemplate(ISender sender, [FromBody] UpdateWorkoutTemplateCommand command, int id)
    {
        Result? result = null;
        //try
        //{
            result = await sender.Send(command);
        //}
        //catch (Exception e)
        //{
            //return Result.Failure([e.Message]);
        //}
        return result ?? Result.Failure(["Failed to create template"]);
    }

    public async Task<Result> DeleteWorkoutTemplate(ISender sender, int id)
    {
        Result? result = null;
        var command = new DeleteMuscleGroupCommand()
        {
            Id = id
        };
        try
        {
            result = await sender.Send(command);
        }
        catch (Exception e)
        {
            return Result.Failure([e.Message]);
        }
        return result ?? Result.Failure(["Failed to create template"]);
        
    }

    public Task<PaginatedList<WorkoutTemplateListDto>> GetPublicTemplates(ISender sender, [AsParameters] GetPublicTemplatesQuery query)
    {
        return sender.Send(query);
    }

    public async Task<WorkoutTemplateDetailsDto> GetWorkoutTemplateDetails(ISender sender, int id)
    {
        var result = await sender.Send(new GetWorkoutTemplateDetailsQuery { Id = id });
        return result;
    }

    public Task<PaginatedList<WorkoutTemplateListDto>> FilterWorkoutTemplates(ISender sender, [AsParameters] FilterWorkoutTemplatesQuery query)
    {
        return sender.Send(query);
    }
}

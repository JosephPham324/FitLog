
using FitLog.Application.Common.Models;
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
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class WorkoutTemplates : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreatePersonalTemplate, "create-personal-template")
            .MapPost(CreateWorkoutTemplate, "create-workout-template")
            .MapPut(UpdateWorkoutTemplate, "update-workout-template/{id}")
            .MapDelete(DeleteWorkoutTemplate, "delete-workout-template/{id}")
            .MapGet(GetPublicTemplates, "get-public-templates")
            .MapGet(GetWorkoutTemplateDetails, "get-workout-template-details/{id}")
            .MapGet(FilterWorkoutTemplates, "filter-workout-templates")
            //.MapGet(SearchWorkoutTemplateByName, "search-workout-templates")
            ;
    }

    public Task<int> CreatePersonalTemplate(ISender sender, [FromBody] CreatePersonalTemplateCommand command)
    {
        return sender.Send(command);
    }

    public Task<int> CreateWorkoutTemplate(ISender sender, [FromBody] CreateWorkoutTemplateCommand command)
    {
        return sender.Send(command);
    }

    public Task<bool> UpdateWorkoutTemplate(ISender sender, [FromBody] UpdateWorkoutTemplateCommand command, int id)
    {
        if (command.Id != id) return Task.FromResult(false);
        return sender.Send(command);
    }

    public Task<bool> DeleteWorkoutTemplate(ISender sender, int id)
    {
        var command = new DeleteWorkoutTemplateCommand(id);
        return sender.Send(command);
    }

    //public async Task<IResult> GetPersonalTemplateById(ISender sender, int id)
    //{
    //    var result = await sender.Send(new GetPersonalTemplateDeQuery { Id = id });
    //    return result is not null ? Results.Ok(result) : Results.NotFound();
    //}

    public Task<PaginatedList<WorkoutTemplateListDto>> GetPublicTemplates(ISender sender, [AsParameters] GetPublicTemplatesQuery query)
    {
        return sender.Send(query);
    }

    public async Task<IResult> GetWorkoutTemplateDetails(ISender sender, int id)
    {
        var result = await sender.Send(new GetWorkoutTemplateDetailsQuery { Id = id });
        return result is not null ? Results.Ok(result) : Results.NotFound();
    }

    public Task<PaginatedList<WorkoutTemplateListDto>> FilterWorkoutTemplates(ISender sender, [AsParameters] FilterWorkoutTemplatesQuery query)
    {
        return sender.Send(query);
    }

    //public Task<PaginatedList<WorkoutTemplateListDto>> SearchWorkoutTemplateByName(ISender sender, [AsParameters] SearchWorkoutTemplateByNameQuery query)
    //{
    //    return sender.Send(query);
    //}
}

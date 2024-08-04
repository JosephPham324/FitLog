using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Equipments.Commands.CreateEquipment;
using FitLog.Application.Equipments.Queries.GetEquipmentDetails;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Application.WorkoutLogs.Commands.CreateWorkoutLog;
using FitLog.Application.WorkoutLogs.Commands.DeleteWorkoutLog;
using FitLog.Application.WorkoutLogs.Commands.UpdateWorkoutLog;
using FitLog.Application.WorkoutLogs.Queries.ExportWorkoutData;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogDetails;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class WorkoutLog : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public WorkoutLog()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }

    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetWorkoutLogsWithPagination, "get-all")
            .MapGet(GetWorkoutHistory, "history")
            .MapGet(GetWorkoutLogDetails, "{WorkoutLogId}")
            .MapPost(CreateWorkoutLog)
            .MapPut(UpdateWorkoutLog, "{id}")
            .MapDelete(DeleteWorkoutLog, "{id}");
        app.MapGroup(this)
            .MapGet(ExportWorkotuData, "export");
    }

    public async Task<PaginatedList<WorkoutLogDTO>> GetWorkoutLogsWithPagination(ISender sender, [AsParameters] GetWorkoutLogsWithPaginationQuery query)
    {
        return await sender.Send(query);
    }
    public async Task<WorkoutLogDetailsDto> GetWorkoutLogDetails(ISender sender, [FromRoute] int WorkoutLogId)
    {
        var UserId = _identityService.Id ?? "";
        var query = new GetWorkoutLogDetailsQuery(
            )
        {
            UserId = UserId,
            WorkoutLogId = WorkoutLogId
        };
        return await sender.Send(query);
    }


    public async Task<object> GetEquipmentById(ISender sender, [AsParameters] GetWorkoutLogsWithPaginationQuery query)
    {
        var result = await sender.Send(query);
        return result;
    }

    public async Task<Result> CreateWorkoutLog(ISender sender, [FromBody] CreateWorkoutLogCommandDTO commandDTO)
    {
        CreateWorkoutLogCommand command = new CreateWorkoutLogCommand(_identityService.Id ?? "", commandDTO); ;

        return await sender.Send(command);
    }

    public async Task<Result> DeleteWorkoutLog(ISender sender, int id, [FromBody] DeleteWorkoutLogCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<Result> UpdateWorkoutLog(ISender sender, int id, [FromBody] UpdateWorkoutLogCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<List<WorkoutLogDTO>> GetWorkoutHistory(ISender sender, [FromQuery] string StartDate, [FromQuery] string EndDate)
    {
        var UserId = _identityService.Id ?? "";
        try
        {
            DateTime.Parse(StartDate);
            DateTime.Parse(EndDate);
        }
        catch (Exception)
        {
            throw new Exception("Please enter valid date");
        }
        var query = new GetWorkoutHistoryQuery(UserId, DateTime.Parse(StartDate), DateTime.Parse(EndDate));
        return await sender.Send(query);
    }

    public async Task<string> ExportWorkotuData(ISender sender, [AsParameters] ExportWorkoutDataQuery query)
    {
        return await sender.Send(query);
    }
}

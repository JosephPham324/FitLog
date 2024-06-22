
using FitLog.Application.Common.Models;
using FitLog.Application.Equipments.Commands.CreateEquipment;
using FitLog.Application.Equipments.Queries.GetEquipmentDetails;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Application.WorkoutLogs.Commands.CreateWorkoutLog;
using FitLog.Application.WorkoutLogs.Commands.DeleteWorkoutLog;
using FitLog.Application.WorkoutLogs.Commands.UpdateWorkoutLog;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class WorkoutLog : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetWorkoutLogsWithPagination, "get-all")
            .MapGet(GetWorkoutHistory, "history")
            .MapPost(CreateExerciseLog)
            .MapPut(UpdateWorkoutLog,"{id}")
            .MapDelete(DeleteWorkoutLog,"{id}");

    }

    public Task<PaginatedList<WorkoutLogDTO>> GetWorkoutLogsWithPagination(ISender sender, [AsParameters] GetWorkoutLogsWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public async Task<object> GetEquipmentById(ISender sender, [AsParameters] GetWorkoutLogsWithPaginationQuery query)
    {
        var result = await sender.Send(query);
        return result;
    }

    public Task<int> CreateExerciseLog(ISender sender, [AsParameters] CreateWorkoutLogCommand command)
    {
        return sender.Send(command);
    }
    public Task<bool> DeleteWorkoutLog(ISender sender, int id, [FromBody] DeleteWorkoutLogCommand command)
    {
        return sender.Send(command);
    }
    public Task<bool> UpdateWorkoutLog(ISender sender, int id, [FromBody] UpdateWorkoutLogCommand command)
    {
        return sender.Send(command);
    }
    public Task<object> GetWorkoutHistory(ISender sender, [AsParameters] GetWorkoutHistoryQuery query)
    {
        return sender.Send(query);
    }
}

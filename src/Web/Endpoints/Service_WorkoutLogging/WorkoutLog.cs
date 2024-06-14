
using FitLog.Application.Common.Models;
using FitLog.Application.Equipments.Commands.CreateEquipment;
using FitLog.Application.Equipments.Queries.GetEquipmentDetails;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Application.WorkoutLogs.Commands.CreateWorkoutLog;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class WorkoutLog : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetWorkoutLogsWithPagination, "get-all")
            .MapPost(CreateExerciseLog);
            
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
}

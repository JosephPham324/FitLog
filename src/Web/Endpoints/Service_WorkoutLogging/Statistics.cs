using FitLog.Application.Statistics_Workout.Queries.GetSummaryStats;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class Statistics : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetWorkoutLogSummary, "summary");
    }
    public async Task<SummaryWorkoutLogStatsDTO> GetWorkoutLogSummary(ISender sender, [AsParameters] GetSummaryStatsQuery query)
    {
        return await sender.Send(query);
    }

    
}

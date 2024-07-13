using FitLog.Application.Statistics_Workout.Queries.GetMuscleEngagement;
using FitLog.Application.Statistics_Workout.Queries.GetSummaryStats;
using FitLog.Application.Statistics_Workout.Queries.GetTotalReps;
using FitLog.Application.Statistics_Workout.Queries.GetTotalTrainingTonnage;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class Statistics : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetWorkoutLogSummary, "summary")
            .MapGet(GetMusclesEngagement, "muscles-engagement")
            .MapGet(GetRepsStats, "total-training-reps")
            .MapGet(GetTonnageStats, "total-training-tonnage");
    }
    public async Task<SummaryWorkoutLogStatsDTO> GetWorkoutLogSummary(ISender sender, [AsParameters] GetSummaryStatsQuery query)
    {
        return await sender.Send(query);
    }

    public async Task<object> GetMusclesEngagement(ISender sender, [AsParameters] GetMuscleEngagementQuery query)
    {
        return await sender.Send(query);
    }
    public async Task<object> GetRepsStats(ISender sender, [AsParameters] GetTotalRepsQuery query)
    {
        return await sender.Send(query);
    }

    public async Task<object> GetTonnageStats(ISender sender, [AsParameters] GetTotalTrainingTonnageQuery query)
    {
        return await sender.Send(query);
    }


}

using FitLog.Application.Statistics_Exercise.Queries.GetExerciseLogHistory;
using FitLog.Application.Statistics_Workout.Queries.GetMuscleEngagement;
using FitLog.Application.Statistics_Workout.Queries.GetSummaryStats;
using FitLog.Application.Statistics_Workout.Queries.GetTotalReps;
using FitLog.Application.Statistics_Workout.Queries.GetTotalTrainingTonnage;
using FitLog.Application.Statistics_Workout.Queries.GetTrainingFrequency;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class Statistics : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetWorkoutLogSummary, "summary")
            .MapGet(GetMusclesEngagement, "muscles-engagement")
            .MapGet(GetRepsStats, "total-training-reps")
            .MapGet(GetTonnageStats, "total-training-tonnage")
            .MapGet(GetTrainingFrequencies, "training-frequency")
            .MapGet(GetExerciseLogHistory, "exercise-log-history");
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
    public async Task<object> GetTrainingFrequencies(ISender sender, [AsParameters] GetTrainingFrequencyQuery query)
    {
        return await sender.Send(query);
    }
    public async Task<object> GetExerciseLogHistory(ISender sender, [AsParameters] GetExerciseLogHistoryQuery query)
    {
        return await sender.Send(query);
    }

}

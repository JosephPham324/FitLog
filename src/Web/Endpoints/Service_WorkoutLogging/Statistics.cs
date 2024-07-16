using FitLog.Application.Statistics_Exercise.Queries.GetExerciseEstimated1RMs;
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseLogHistory;
using FitLog.Application.Statistics_Workout.Queries.GetMuscleEngagement;
using FitLog.Application.Statistics_Workout.Queries.GetSummaryStats;
using FitLog.Application.Statistics_Workout.Queries.GetTotalReps;
using FitLog.Application.Statistics_Workout.Queries.GetTotalTrainingTonnage;
using FitLog.Application.Statistics_Workout.Queries.GetTrainingFrequency;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;

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
            .MapGet(GetExerciseLogHistory, "exercise-log-history")
            .MapGet(GetEstimated1RM, "estimated1RM");
    }
    public async Task<Dictionary<DateTime, SummaryWorkoutLogStatsDTO>> GetWorkoutLogSummary(ISender sender, [AsParameters] GetSummaryStatsQuery query)
    {
        return await sender.Send(query);
    }

    public async Task<List<MuscleEngagementDTO>> GetMusclesEngagement(ISender sender, [AsParameters] GetMuscleEngagementQuery query)
    {
        return await sender.Send(query);
    }
    public async Task<Dictionary<DateTime, int>> GetRepsStats(ISender sender, [AsParameters] GetTotalRepsQuery query)
    {
        return await sender.Send(query);
    }

    public async Task<Dictionary<DateTime, double>> GetTonnageStats(ISender sender, [AsParameters] GetTotalTrainingTonnageQuery query)
    {
        return await sender.Send(query);
    }
    public async Task<Dictionary<DateTime, int>> GetTrainingFrequencies(ISender sender, [AsParameters] GetTrainingFrequencyQuery query)
    {
        return await sender.Send(query);
    }
    public async Task<IEnumerable<ExerciseLogDTO>> GetExerciseLogHistory(ISender sender, [AsParameters] GetExerciseLogHistoryQuery query)
    {
        return await sender.Send(query);
    }


    public async Task<object> GetEstimated1RM(ISender sender, [AsParameters] GetExerciseEstimated1RMsQuery query)
    {
        return await sender.Send(query);
    }

}

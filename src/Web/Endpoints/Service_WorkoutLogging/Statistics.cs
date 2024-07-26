using FitLog.Application.Common.Interfaces;
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseEstimated1RMs;
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseLogHistory;
using FitLog.Application.Statistics_Exercise.Queries.GetExercisesWithHistory;
using FitLog.Application.Statistics_Workout.Queries.GetMuscleEngagement;
using FitLog.Application.Statistics_Workout.Queries.GetSummaryStats;
using FitLog.Application.Statistics_Workout.Queries.GetTotalReps;
using FitLog.Application.Statistics_Workout.Queries.GetTotalTrainingTonnage;
using FitLog.Application.Statistics_Workout.Queries.GetTrainingFrequency;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class Statistics : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public Statistics()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }


    public override void Map(WebApplication app)
    {
        var statsGroup =
        app.MapGroup(this)
            .RequireAuthorization();

        statsGroup
            .MapGroup("overall")
            .MapGet(GetWorkoutLogSummary, "summary")
            .MapGet(GetMusclesEngagement, "muscles-engagement")
            .MapGet(GetRepsStats, "total-training-reps")
            .MapGet(GetTonnageStats, "total-training-tonnage")
            .MapGet(GetTrainingFrequencies, "training-frequency");

        statsGroup
            .MapGroup("exercise")
            .MapGet(GetExerciseLogHistory, "exercise-log-history")
            .MapGet(GetEstimated1RM, "estimated1RM")
            .MapGet(GetExercisesWithHistory, "logged-exercises");
    }
    public async Task<Dictionary<DateTime, SummaryWorkoutLogStatsDTO>> GetWorkoutLogSummary(ISender sender, [FromQuery] string TimeFrame)
    {
        var UserId = _identityService.Id ?? "";
        var query = new GetSummaryStatsQuery
        {
            UserId = UserId,
            TimeFrame = TimeFrame
        };
        return await sender.Send(query);
    }

    public async Task<Dictionary<DateTime, List<MuscleEngagementDTO>>> GetMusclesEngagement(ISender sender, [FromQuery] string TimeFrame)
    {
        var UserId = _identityService.Id ?? "";
        var query = new GetMuscleEngagementQuery
        {
            UserId = UserId,
            TimeFrame = TimeFrame
        };
        return await sender.Send(query);
    }

    public async Task<Dictionary<DateTime, int>> GetRepsStats(ISender sender, [FromQuery] string TimeFrame)
    {
        var UserId = _identityService.Id ?? "";
        var query = new GetTotalRepsQuery()
        {
            UserId = UserId,
            TimeFrame = TimeFrame
        };
        return await sender.Send(query);
    }

    public async Task<Dictionary<DateTime, double>> GetTonnageStats(ISender sender, [FromQuery] string TimeFrame)
    {
        var UserId = _identityService.Id ?? "";
        var query = new GetTotalTrainingTonnageQuery
        {
            UserId = UserId,
            TimeFrame = TimeFrame
        };
        return await sender.Send(query);
    }

    public async Task<Dictionary<DateTime, int>> GetTrainingFrequencies(ISender sender, [FromQuery] string TimeFrame)
    {
        var UserId = _identityService.Id ?? "";
        var query = new GetTrainingFrequencyQuery
        {
            UserId = UserId,
            TimeFrame = TimeFrame
        };
        return await sender.Send(query);
    }

    public async Task<IEnumerable<ExerciseLogDTO>> GetExerciseLogHistory(ISender sender, [AsParameters] GetExerciseLogHistoryQuery query)
    {
        query.UserId = _identityService.Id ?? "";

        return await sender.Send(query);
    }


    public async Task<object> GetEstimated1RM(ISender sender, [AsParameters] GetExerciseEstimated1RMsQuery query)
    {
        query.UserId = _identityService.Id ?? "";

        return await sender.Send(query);
    }

    public async Task<List<ExerciseHistoryEntry>> GetExercisesWithHistory(ISender sender)
    {
        var UserId = _identityService.Id ?? "";
        var query = new GetExercisesWithHistoryQuery(UserId);

        return await sender.Send(query);
    }
}

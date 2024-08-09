using FitLog.Application.Common.Interfaces;
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseEstimated1RMs;
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseLogHistory;
using FitLog.Application.Statistics_Exercise.Queries.GetExercisesWithHistory;
using FitLog.Application.Statistics_Exercise.Queries.GetRecordsHistory;
using FitLog.Application.Statistics_Exercise.Queries.GetTotalRepsForExercise;
using FitLog.Application.Statistics_Exercise.Queries.GetTotalTrainingTonnageForExercise;
using FitLog.Application.Statistics_Workout.Queries.GetMuscleEngagement;
using FitLog.Application.Statistics_Workout.Queries.GetSummaryStats;
using FitLog.Application.Statistics_Workout.Queries.GetTotalReps;
using FitLog.Application.Statistics_Workout.Queries.GetTotalTrainingTonnage;
using FitLog.Application.Statistics_Workout.Queries.GetTrainingFrequency;
using FitLog.Application.Users.Queries.UserWithCoachServiceQuery;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Entities;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging
{
    public class Statistics : EndpointGroupBase
    {
        private readonly IUserTokenService _tokenService;
        private readonly IUser _identityService;

        public Statistics()
        {
            _tokenService = new CurrentUserFromToken(new HttpContextAccessor());
            _identityService = new CurrentUser(new HttpContextAccessor());
        }

        public override void Map(WebApplication app)
        {
            // Personal statistics
            var personalStats = app.MapGroup(this)
                //.RequireAuthorization("MemberOnly")
                ;

            personalStats
                .MapGroup("overall")
                .MapGet(GetWorkoutLogSummary, "summary")
                .MapGet(GetMusclesEngagement, "muscles-engagement")
                .MapGet(GetRepsStats, "total-training-reps")
                .MapGet(GetTonnageStats, "total-training-tonnage")
                .MapGet(GetTrainingFrequencies, "training-frequency");

            personalStats
                .MapGroup("exercise")
                .MapGet(GetExerciseLogHistory, "exercise-log-history/{ExerciseId}")
                .MapGet(GetEstimated1RM, "estimated1RM")
                .MapGet(GetExercisesWithHistory, "logged-exercises")
                .MapGet(GetExerciseRecords, "{ExerciseId}/records")
                .MapGet(GetExerciseTotalReps, "{ExerciseId}/total-reps")
                .MapGet(GetExerciseTotalTonnage, "{ExerciseId}/total-tonnage");

            // User statistics
            var userStats = app.MapGroup(this)
               //.RequireAuthorization("CoachOnly")
               ;
            userStats
                .MapGroup("user/{id}/overall")
                .MapGet(GetUserWorkoutLogSummary, "summary")
                .MapGet(GetUserMusclesEngagement, "muscles-engagement")
                .MapGet(GetUserRepsStats, "total-training-reps")
                .MapGet(GetUserTonnageStats, "total-training-tonnage")
                .MapGet(GetUserTrainingFrequencies, "training-frequency");

            userStats
                .MapGroup("user/{id}/exercise")
                .MapGet(GetUserExerciseLogHistory, "exercise-log-history")
                .MapGet(GetUserEstimated1RM, "estimated1RM")
                .MapGet(GetUserExercisesWithHistory, "logged-exercises");
        }

        // Personal statistics methods
        #region Overal stats
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

        #endregion
        #region Exercise stats
        public async Task<IEnumerable<ExerciseLogDTO>> GetExerciseLogHistory(ISender sender, [FromRoute] int ExerciseId)
        {

            var UserId = _identityService.Id ?? "";

            GetExerciseLogHistoryQuery query = new GetExerciseLogHistoryQuery()
            {
                UserId = UserId,
                ExerciseId = ExerciseId
            };

            return await sender.Send(query);
        }

        public async Task<Dictionary<DateTime, OneRepMaxRecord>> GetEstimated1RM(ISender sender, [AsParameters] GetExerciseEstimated1RMsQuery query)
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

        public async Task<PersonalRecordDTO> GetExerciseRecords(ISender sender, [FromRoute] int ExerciseId)
        {
            var UserId = _identityService.Id ?? "";
            var query = new GetRecordsHistoryQuery()
            {
                UserId = UserId,
                ExerciseId = ExerciseId
            };

            return await sender.Send(query);
        }

        public async Task<Dictionary<DateTime, int>> GetExerciseTotalReps(ISender sender, [FromRoute] int ExerciseId, [FromQuery] string TimeFrame)
        {
            var UserId = _identityService.Id ?? "";
            var query = new GetTotalRepsForExerciseQuery()
            {
                UserId = UserId,
                TimeFrame = TimeFrame,
                ExerciseId = ExerciseId
            };
            return await sender.Send(query);
        }

        public async Task<Dictionary<DateTime, double>> GetExerciseTotalTonnage(ISender sender, [FromRoute] int ExerciseId, [FromQuery] string TimeFrame)
        {
            var UserId = _identityService.Id ?? "";
            var query = new GetTotalTrainingTonnageForExerciseQuery()
            {
                UserId = UserId,
                TimeFrame = TimeFrame,
                ExerciseId = ExerciseId
            };
            return await sender.Send(query);
        }
        #endregion
        #region Users
        // User statistics methods
        public async Task<Dictionary<DateTime, SummaryWorkoutLogStatsDTO>> GetUserWorkoutLogSummary(ISender sender, [FromRoute] string id, [FromQuery] string TimeFrame)
        {
            var currentUserId = _identityService.Id ?? "";

            var isUserCoachedQuery = new UserWithCoachServiceQueryQuery
            {
                UserId = id,
                CoachId = currentUserId
            };
            bool isUserCoached = await sender.Send(isUserCoachedQuery);

            if (!isUserCoached)
            {
                throw new UnauthorizedAccessException("User is not coached by the current user");
            }

            var query = new GetSummaryStatsQuery
            {
                UserId = id,
                TimeFrame = TimeFrame
            };
            return await sender.Send(query);
        }

        public async Task<Dictionary<DateTime, List<MuscleEngagementDTO>>> GetUserMusclesEngagement(ISender sender, [FromRoute] string id, [FromQuery] string TimeFrame)
        {
            var currentUserId = _identityService.Id ?? "";

            var isUserCoachedQuery = new UserWithCoachServiceQueryQuery
            {
                UserId = id,
                CoachId = currentUserId
            };
            bool isUserCoached = await sender.Send(isUserCoachedQuery);

            if (!isUserCoached)
            {
                throw new UnauthorizedAccessException("User is not coached by the current user");
            }

            var query = new GetMuscleEngagementQuery
            {
                UserId = id,
                TimeFrame = TimeFrame
            };
            return await sender.Send(query);
        }

        public async Task<Dictionary<DateTime, int>> GetUserRepsStats(ISender sender, [FromRoute] string id, [FromQuery] string TimeFrame)
        {
            var currentUserId = _identityService.Id ?? "";

            var isUserCoachedQuery = new UserWithCoachServiceQueryQuery
            {
                UserId = id,
                CoachId = currentUserId
            };
            bool isUserCoached = await sender.Send(isUserCoachedQuery);

            if (!isUserCoached)
            {
                throw new UnauthorizedAccessException("User is not coached by the current user");
            }

            var query = new GetTotalRepsQuery()
            {
                UserId = id,
                TimeFrame = TimeFrame
            };
            return await sender.Send(query);
        }

        public async Task<Dictionary<DateTime, double>> GetUserTonnageStats(ISender sender, [FromRoute] string id, [FromQuery] string TimeFrame)
        {
            var currentUserId = _identityService.Id ?? "";

            var isUserCoachedQuery = new UserWithCoachServiceQueryQuery
            {
                UserId = id,
                CoachId = currentUserId
            };
            bool isUserCoached = await sender.Send(isUserCoachedQuery);

            if (!isUserCoached)
            {
                throw new UnauthorizedAccessException("User is not coached by the current user");
            }

            var query = new GetTotalTrainingTonnageQuery
            {
                UserId = id,
                TimeFrame = TimeFrame
            };
            return await sender.Send(query);
        }

        public async Task<Dictionary<DateTime, int>> GetUserTrainingFrequencies(ISender sender, [FromRoute] string id, [FromQuery] string TimeFrame)
        {
            var currentUserId = _identityService.Id ?? "";

            var isUserCoachedQuery = new UserWithCoachServiceQueryQuery
            {
                UserId = id,
                CoachId = currentUserId
            };
            bool isUserCoached = await sender.Send(isUserCoachedQuery);

            if (!isUserCoached)
            {
                throw new UnauthorizedAccessException("User is not coached by the current user");
            }

            var query = new GetTrainingFrequencyQuery
            {
                UserId = id,
                TimeFrame = TimeFrame
            };
            return await sender.Send(query);
        }

        public async Task<IEnumerable<ExerciseLogDTO>> GetUserExerciseLogHistory(ISender sender, [FromRoute] string id, [AsParameters] GetExerciseLogHistoryQuery query)
        {
            var currentUserId = _identityService.Id ?? "";

            var isUserCoachedQuery = new UserWithCoachServiceQueryQuery
            {
                UserId = id,
                CoachId = currentUserId
            };
            bool isUserCoached = await sender.Send(isUserCoachedQuery);

            if (!isUserCoached)
            {
                throw new UnauthorizedAccessException("User is not coached by the current user");
            }

            query.UserId = id;

            return await sender.Send(query);
        }

        public async Task<object> GetUserEstimated1RM(ISender sender, [FromRoute] string id, [AsParameters] GetExerciseEstimated1RMsQuery query)
        {
            var currentUserId = _identityService.Id ?? "";

            var isUserCoachedQuery = new UserWithCoachServiceQueryQuery
            {
                UserId = id,
                CoachId = currentUserId
            };
            bool isUserCoached = await sender.Send(isUserCoachedQuery);

            if (!isUserCoached)
            {
                throw new UnauthorizedAccessException("User is not coached by the current user");
            }

            query.UserId = id;

            return await sender.Send(query);
        }

        public async Task<List<ExerciseHistoryEntry>> GetUserExercisesWithHistory(ISender sender, [FromRoute] string id)
        {
            var currentUserId = _identityService.Id ?? "";

            var isUserCoachedQuery = new UserWithCoachServiceQueryQuery
            {
                UserId = id,
                CoachId = currentUserId
            };
            bool isUserCoached = await sender.Send(isUserCoachedQuery);

            if (!isUserCoached)
            {
                throw new UnauthorizedAccessException("User is not coached by the current user");
            }

            var query = new GetExercisesWithHistoryQuery(id);

            return await sender.Send(query);
        }
        #endregion
    }
}

using FitLog.Application.Common.Extensions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.ValidationRules;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;

namespace FitLog.Application.Statistics_Workout.Queries.GetMuscleEngagement
{
    public class MuscleEngagementDTO
    {
        public string Muscle { get; set; } = string.Empty;
        public int Sets { get; set; }
    }

    public record GetMuscleEngagementQuery : IRequest<Dictionary<DateTime, List<MuscleEngagementDTO>>>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;
        public string TimeFrame { get; set; } = string.Empty;
    }

    public class GetMuscleEngagementQueryValidator : AbstractValidator<GetMuscleEngagementQuery>
    {
        public GetMuscleEngagementQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.TimeFrame).NotEmpty().WithMessage("TimeFrame is required.")
                                      .Must(ValidationRules.ValidTimeFrame).WithMessage("Invalid TimeFrame.");
        }
    }

    public class GetMuscleEngagementQueryHandler : IRequestHandler<GetMuscleEngagementQuery, Dictionary<DateTime, List<MuscleEngagementDTO>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public GetMuscleEngagementQueryHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Dictionary<DateTime, List<MuscleEngagementDTO>>> Handle(GetMuscleEngagementQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset endDate = DateTimeOffset.Now;
            DateTimeOffset startDate = new DateTimeOffset(new DateTime(1900, 1, 1)); // Fetch all history from January 1, 1900

            var workoutHistoryQuery = new GetWorkoutHistoryQuery(request.UserId, startDate.DateTime, endDate.DateTime);
            var workoutLogs = await _mediator.Send(workoutHistoryQuery, cancellationToken) as List<WorkoutLogDTO> ?? new List<WorkoutLogDTO>();

            var muscleEngagementByPeriod = new Dictionary<DateTime, Dictionary<string, int>>();

            foreach (var log in workoutLogs)
            {
                DateTime periodStart;
                switch (request.TimeFrame.ToUpperInvariant())
                {
                    case string weekly when weekly == TimeFrames.Weekly.ToUpperInvariant():
                        periodStart = log.Created.LocalDateTime.StartOfWeek(DayOfWeek.Monday);
                        break;
                    case string monthly when monthly == TimeFrames.Monthly.ToUpperInvariant():
                        periodStart = new DateTime(log.Created.Year, log.Created.Month, 1);
                        break;
                    case string yearly when yearly == TimeFrames.Yearly.ToUpperInvariant():
                        periodStart = new DateTime(log.Created.Year, 1, 1);
                        break;
                    default:
                        throw new ArgumentException("Invalid TimeFrame", nameof(request.TimeFrame));
                }

                if (!muscleEngagementByPeriod.ContainsKey(periodStart))
                {
                    muscleEngagementByPeriod[periodStart] = new Dictionary<string, int>();
                }

                var muscleEngagement = muscleEngagementByPeriod[periodStart];

                foreach (var exerciseLog in log.ExerciseLogs)
                {
                    var exercise = await _context.Exercises
                        .Include(e => e.ExerciseMuscleGroups)
                        .ThenInclude(emg => emg.MuscleGroup)
                        .FirstOrDefaultAsync(e => e.ExerciseId == exerciseLog.ExerciseId, cancellationToken);

                    if (exercise != null)
                    {
                        foreach (var exerciseMuscleGroup in exercise.ExerciseMuscleGroups)
                        {
                            var muscleGroup = exerciseMuscleGroup.MuscleGroup;
                            if (muscleGroup == null)
                            {
                                continue;
                            }

                            var muscleGroupName = muscleGroup.MuscleGroupName ?? "";
                            if (muscleEngagement.ContainsKey(muscleGroupName))
                            {
                                muscleEngagement[muscleGroupName] += exerciseLog.NumberOfSets ?? 0;
                            }
                            else
                            {
                                muscleEngagement[muscleGroupName] = exerciseLog.NumberOfSets ?? 0;
                            }
                        }
                    }
                }
            }

            return muscleEngagementByPeriod.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(me => new MuscleEngagementDTO { Muscle = me.Key, Sets = me.Value }).ToList()
            );
        }
    }
}

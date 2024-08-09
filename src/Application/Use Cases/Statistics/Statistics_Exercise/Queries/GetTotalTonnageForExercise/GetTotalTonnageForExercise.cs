using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using FitLog.Application.Common.Extensions;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Domain.Constants;
using FitLog.Application.Common.ValidationRules;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;

namespace FitLog.Application.Statistics_Exercise.Queries.GetTotalTrainingTonnageForExercise
{
    public record GetTotalTrainingTonnageForExerciseQuery : IRequest<Dictionary<DateTime, double>>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;
        public string TimeFrame { get; set; } = string.Empty;
        public int ExerciseId { get; init; }
    }

    public class GetTotalTrainingTonnageForExerciseQueryValidator : AbstractValidator<GetTotalTrainingTonnageForExerciseQuery>
    {
        public GetTotalTrainingTonnageForExerciseQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.TimeFrame).NotEmpty().WithMessage("TimeFrame is required.")
                                      .Must(ValidationRules.ValidTimeFrame).WithMessage("Invalid TimeFrame.");

            RuleFor(x => x.ExerciseId).NotEmpty()
                .WithMessage("ExerciseId is required.")
                .GreaterThan(0)
                .WithMessage("ExerciseId must be more than 0");
        }
    }
    public class GetTotalTrainingTonnageForExerciseQueryHandler : IRequestHandler<GetTotalTrainingTonnageForExerciseQuery, Dictionary<DateTime, double>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public GetTotalTrainingTonnageForExerciseQueryHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Dictionary<DateTime, double>> Handle(GetTotalTrainingTonnageForExerciseQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset endDate = DateTimeOffset.Now;
            DateTimeOffset startDate = new DateTimeOffset(new DateTime(1900, 1, 1)); // Fetch all history from January 1, 1900

            var workoutHistoryQuery = new GetWorkoutHistoryQuery(request.UserId, startDate.DateTime, endDate.DateTime);
            var workoutLogs = await _mediator.Send(workoutHistoryQuery, cancellationToken) as List<WorkoutLogDTO> ?? new List<WorkoutLogDTO>();

            var totalTonnageByPeriod = new Dictionary<DateTime, double>();

            foreach (var log in workoutLogs)
            {
                DateTime periodStart;
                switch (request.TimeFrame.ToUpperInvariant())
                {
                    case string weekly when weekly == TimeFrames.Weekly.ToUpperInvariant():
                        periodStart = log.Created.UtcDateTime.StartOfWeek(DayOfWeek.Monday);
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

                if (!totalTonnageByPeriod.ContainsKey(periodStart))
                {
                    totalTonnageByPeriod[periodStart] = 0;
                }

                foreach (var exerciseLog in log.ExerciseLogs)
                {
                    if (exerciseLog.ExerciseId == request.ExerciseId)
                    {
                        var weights = exerciseLog.GetWeightsUsed();
                        var reps = exerciseLog.GetNumberOfReps();

                        if (weights != null && reps != null)
                        {
                            for (int i = 0; i < weights.Count; i++)
                            {
                                totalTonnageByPeriod[periodStart] += weights[i] * (reps.Count > i ? reps[i] : 0);
                            }
                        }
                    }
                }
            }

            return totalTonnageByPeriod;
        }
    }
}

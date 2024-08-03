using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace FitLog.Application.Statistics_Exercise.Queries.GetRecordsHistory
{
    public class PersonalRecordDTO
    {
        public double Actual1RepMax { get; set; }
        public double Estimated1RepMax { get; set; }
        public double MaxVolume { get; set; }
        public Dictionary<int, BestPerformanceDTO>? BestPerformances { get; set; } = null;
    }

    public class BestPerformanceDTO
    {
        public double Weight { get; set; }
        public DateTime Date { get; set; }
    }

    public record GetRecordsHistoryQuery : IRequest<PersonalRecordDTO>
    {
        public string UserId { get; init; } = string.Empty;
        public int ExerciseId { get; init; }
    }

    public class GetRecordsHistoryQueryValidator : AbstractValidator<GetRecordsHistoryQuery>
    {
        public GetRecordsHistoryQueryValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty();
            RuleFor(v => v.ExerciseId)
                .NotEmpty();
        }
    }

    public class GetRecordsHistoryQueryHandler : IRequestHandler<GetRecordsHistoryQuery, PersonalRecordDTO>
    {
        private readonly IApplicationDbContext _context;

        public GetRecordsHistoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PersonalRecordDTO> Handle(GetRecordsHistoryQuery request, CancellationToken cancellationToken)
        {
            var exerciseLogs = await _context.ExerciseLogs
                .Include(el => el.WorkoutLog)
                .Where(el => el.WorkoutLog != null && el.WorkoutLog.CreatedBy == request.UserId && el.ExerciseId == request.ExerciseId)
                .ToListAsync(cancellationToken);

            if (exerciseLogs == null)
            {
                throw new NotFoundException(nameof(ExerciseLog), request.ExerciseId + "");
            }

            var bestPerformances = new Dictionary<int, BestPerformanceDTO>();
            double actual1RepMax = 0;
            double estimated1RepMax = 0;
            double maxVolume = 0;

            foreach (var log in exerciseLogs)
            {
                if (log.WeightsUsedValue == null || log.NumberOfRepsValue == null)
                    continue;

                for (int i = 0; i < log.WeightsUsedValue.Count; i++)
                {
                    var weight = log.WeightsUsedValue[i];
                    var reps = log.NumberOfRepsValue[i];

                    if (weight > actual1RepMax)
                    {
                        actual1RepMax = weight;
                    }

                    var estimated1RM = weight * (1 + reps / 30.0);
                    if (estimated1RM > estimated1RepMax)
                    {
                        estimated1RepMax = estimated1RM;
                    }

                    maxVolume += weight * reps;

                    if (!bestPerformances.ContainsKey(reps))
                    {
                        bestPerformances[reps] = new BestPerformanceDTO { Weight = weight, Date = log.DateCreated };
                    }
                    else if (weight > bestPerformances[reps].Weight)
                    {
                        bestPerformances[reps] = new BestPerformanceDTO { Weight = weight, Date = log.DateCreated };
                    }
                }
            }

            return new PersonalRecordDTO
            {
                Actual1RepMax = actual1RepMax,
                Estimated1RepMax = estimated1RepMax,
                MaxVolume = maxVolume,
                BestPerformances = bestPerformances
            };
        }
    }
}

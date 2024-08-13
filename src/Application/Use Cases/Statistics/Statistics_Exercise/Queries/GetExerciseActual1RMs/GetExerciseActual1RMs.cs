using FitLog.Application.Common.Interfaces;
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseLogHistory;
using FitLog.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Statistics_Exercise.Queries.GetExerciseActual1RMs;

public record GetExerciseActual1RMsQuery : IRequest<Dictionary<DateTime, double>>
{
    public string UserId { get; init; } = string.Empty;
    public int ExerciseId { get; init; }
}

public class GetExerciseActual1RMsQueryValidator : AbstractValidator<GetExerciseActual1RMsQuery>
{
    public GetExerciseActual1RMsQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty();
        RuleFor(v => v.ExerciseId)
            .NotEmpty()
            .WithMessage("ExerciseId is required.")
            .GreaterThan(0)
            .WithMessage("ExerciseId must be more than 0");
    }
}

public class GetExerciseActual1RMsQueryHandler : IRequestHandler<GetExerciseActual1RMsQuery, Dictionary<DateTime, double>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetExerciseActual1RMsQueryHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Dictionary<DateTime, double>> Handle(GetExerciseActual1RMsQuery request, CancellationToken cancellationToken)
    {
        // Get exercise log history
        var logsHistoryQuery = new GetExerciseLogHistoryQuery
        {
            UserId = request.UserId,
            ExerciseId = request.ExerciseId
        };
        var logsHistory = await _mediator.Send(logsHistoryQuery, cancellationToken);

        if (logsHistory == null || !logsHistory.Any())
            throw new NotFoundException(nameof(ExerciseLog), "No record found for this exercise");

        // Process logs to find the maximum 1RM for each date
        var actual1RMByDate = new Dictionary<DateTime, double>();

        foreach (var log in logsHistory)
        {
            var weights = log.GetWeightsUsed();
            var reps = log.GetNumberOfReps();

            if (weights == null || reps == null || weights.Count != reps.Count)
                continue;

            for (int i = 0; i < reps.Count; i++)
            {
                if (reps[i] == 1) // Check if the rep is exactly 1
                {
                    var weight = weights[i];
                    var logDate = log.DateCreated.Date;

                    if (actual1RMByDate.ContainsKey(logDate))
                    {
                        // Compare with the existing 1RM for that date and take the maximum
                        if (weight > actual1RMByDate[logDate])
                        {
                            actual1RMByDate[logDate] = weight;
                        }
                    }
                    else
                    {
                        // Add new record for the date
                        actual1RMByDate.Add(logDate, weight);
                    }
                }
            }
        }

        return actual1RMByDate;
    }
}

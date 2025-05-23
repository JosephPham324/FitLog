﻿using System.Text.Json.Serialization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Entities;

namespace FitLog.Application.Statistics_Exercise.Queries.GetExerciseLogHistory;

public record GetExerciseLogHistoryQuery : IRequest<IEnumerable<ExerciseLogDTO>>
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public int ExerciseId { get; set; }
}

public class GetExerciseLogHistoryQueryValidator : AbstractValidator<GetExerciseLogHistoryQuery>
{
    public GetExerciseLogHistoryQueryValidator()
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

public class GetExerciseLogHistoryQueryHandler : IRequestHandler<GetExerciseLogHistoryQuery, IEnumerable<ExerciseLogDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetExerciseLogHistoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ExerciseLogDTO>> Handle(GetExerciseLogHistoryQuery request, CancellationToken cancellationToken)
    {
        var exerciseLogs = await _context.ExerciseLogs
                .Include(el => el.WorkoutLog)
            .Where(el => el.WorkoutLog != null && el.WorkoutLog.CreatedBy != null && el.WorkoutLog.CreatedBy.Equals(request.UserId))
            .Where(el => el.ExerciseId == request.ExerciseId)
            .Include(el => el.Exercise)
            .ToListAsync(cancellationToken);
        if (exerciseLogs.Count == 0)
            throw new NotFoundException(nameof(ExerciseLog), request.ExerciseId + "");
        return _mapper.Map<List<ExerciseLogDTO>>(exerciseLogs);
    }
}

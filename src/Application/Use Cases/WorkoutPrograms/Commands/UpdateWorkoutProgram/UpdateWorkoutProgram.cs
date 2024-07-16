using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.WorkoutPrograms.Commands.UpdateWorkoutProgram;

public record UpdateWorkoutProgramCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string ProgramName { get; set; } = "";
    public string? ProgramThumbnail { get; set; }
    public int? NumberOfWeeks { get; set; }
    public int? DaysPerWeek { get; set; }
    public string? Goal { get; set; }
    public string? ExperienceLevel { get; set; }
    public string? GymType { get; set; }
    public string? MusclesPriority { get; set; }
    public string? AgeGroup { get; set; }
    public bool? PublicProgram { get; set; }
}

public class UpdateWorkoutProgramCommandValidator : AbstractValidator<UpdateWorkoutProgramCommand>
{
    public UpdateWorkoutProgramCommandValidator()
    {
    }
}

public class UpdateWorkoutProgramCommandHandler : IRequestHandler<UpdateWorkoutProgramCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateWorkoutProgramCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateWorkoutProgramCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Programs.FindAsync(request.Id);

        if (entity == null)
        {
            return Result.Failure(["Workout program not found"]);
        }

        entity.ProgramName = request.ProgramName;
        entity.ProgramThumbnail = request.ProgramThumbnail;
        entity.NumberOfWeeks = request.NumberOfWeeks;
        entity.DaysPerWeek = request.DaysPerWeek;
        entity.Goal = request.Goal;
        entity.ExperienceLevel = request.ExperienceLevel;
        entity.GymType = request.GymType;
        entity.MusclesPriority = request.MusclesPriority;
        entity.AgeGroup = request.AgeGroup;
        entity.PublicProgram = request.PublicProgram;
        entity.LastModified = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}

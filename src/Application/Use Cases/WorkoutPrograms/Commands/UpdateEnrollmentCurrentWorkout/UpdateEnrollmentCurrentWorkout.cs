using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.WorkoutPrograms.Commands.UpdateEnrollmentCurrentWorkout;

public record UpdateEnrollmentCurrentWorkoutCommand : IRequest<Result>
{
    public int EnrollmentId { get; init; }
    public int CurrentWeekNo { get; init; }
    public int CurrentWorkoutOrder { get; init; }
}

public class UpdateEnrollmentCurrentWorkoutCommandValidator : AbstractValidator<UpdateEnrollmentCurrentWorkoutCommand>
{
    public UpdateEnrollmentCurrentWorkoutCommandValidator()
    {
        RuleFor(x => x.EnrollmentId).GreaterThan(0).WithMessage("Enrollment ID must be greater than 0.");
        RuleFor(x => x.CurrentWeekNo).GreaterThan(0).WithMessage("Current week number must be greater than 0.");
        RuleFor(x => x.CurrentWorkoutOrder).GreaterThan(0).WithMessage("Current workout order must be greater than 0.");
    }
}

public class UpdateEnrollmentCurrentWorkoutCommandHandler : IRequestHandler<UpdateEnrollmentCurrentWorkoutCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateEnrollmentCurrentWorkoutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateEnrollmentCurrentWorkoutCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _context.ProgramEnrollments.FindAsync(request.EnrollmentId);

        if (enrollment == null)
        {
            return Result.Failure(new[] { "Enrollment not found." });
        }

        enrollment.CurrentWeekNo = request.CurrentWeekNo;
        enrollment.CurrentWorkoutOrder = request.CurrentWorkoutOrder;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}

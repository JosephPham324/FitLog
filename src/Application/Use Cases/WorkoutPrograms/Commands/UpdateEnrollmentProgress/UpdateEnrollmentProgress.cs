using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.WorkoutPrograms.Commands.UpdateEnrollmentProgress;

public record UpdateEnrollmentProgressCommand : IRequest<Result>
{
    public int EnrollmentId { get; init; }
    public Dictionary<int, Domain.Entities.ProgramEnrollment.WorkoutProgress> WorkoutsProgress { get; init; } = new();
}

public class UpdateEnrollmentProgressCommandValidator : AbstractValidator<UpdateEnrollmentProgressCommand>
{
    public UpdateEnrollmentProgressCommandValidator()
    {
        RuleFor(x => x.EnrollmentId).GreaterThan(0).WithMessage("Enrollment ID must be greater than 0.");
        RuleFor(x => x.WorkoutsProgress).NotNull().WithMessage("Workouts progress must not be null.");
    }
}

public class UpdateEnrollmentProgressCommandHandler : IRequestHandler<UpdateEnrollmentProgressCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateEnrollmentProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateEnrollmentProgressCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _context.ProgramEnrollments.FindAsync(request.EnrollmentId);

        if (enrollment == null)
        {
            return Result.Failure(new[] { "Enrollment not found." });
        }

        enrollment.WorkoutsProgress = request.WorkoutsProgress;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}

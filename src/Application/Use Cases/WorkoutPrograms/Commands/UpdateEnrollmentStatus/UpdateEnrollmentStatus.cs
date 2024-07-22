using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Constants;

namespace FitLog.Application.WorkoutPrograms.Commands.UpdateEnrollmentStatus;

public record UpdateEnrollmentStatusCommand : IRequest<Result>
{
    public int EnrollmentId { get; init; }
    public string NewStatus { get; init; } = "";
}

public class UpdateEnrollmentStatusCommandValidator : AbstractValidator<UpdateEnrollmentStatusCommand>
{
    public UpdateEnrollmentStatusCommandValidator()
    {
        RuleFor(x => x.EnrollmentId).GreaterThan(0).WithMessage("Enrollment ID must be greater than 0.");
        RuleFor(x => x.NewStatus).NotEmpty().WithMessage("New status must not be empty.")
            .Must(status => EnrollmentStatuses.All.Contains(status)).WithMessage("Invalid status.");
    }
}

public class UpdateEnrollmentStatusCommandHandler : IRequestHandler<UpdateEnrollmentStatusCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateEnrollmentStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateEnrollmentStatusCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _context.ProgramEnrollments.FindAsync(request.EnrollmentId);

        if (enrollment == null)
        {
            return Result.Failure(new[] { "Enrollment not found." });
        }

        enrollment.Status = request.NewStatus;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}

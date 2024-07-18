using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutPrograms.Commands.EnrollProgram;

public record EnrollProgramCommand : IRequest<Result>
{
    public string UserId { get; init; } = "";
    public int ProgramId { get; init; }
}

public class EnrollProgramCommandValidator : AbstractValidator<EnrollProgramCommand>
{
    public EnrollProgramCommandValidator()
    {
        RuleFor(x => x.ProgramId).GreaterThan(0).WithMessage("Program ID must be greater than 0.");
    }
}

public class EnrollProgramCommandHandler : IRequestHandler<EnrollProgramCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public EnrollProgramCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(EnrollProgramCommand request, CancellationToken cancellationToken)
    {
        var program = await _context.Programs.FindAsync(request.ProgramId);

        if (program == null)
        {
            return Result.Failure(["Program not found."]);
        }

        var enrollment = new ProgramEnrollment
        {
            UserId = request.UserId,
            ProgramId = request.ProgramId,
            EnrolledDate = DateTime.UtcNow,
            Status = EnrollmentStatuses.Enrolled,
            CurrentWeekNo = 1,
            CurrentWorkoutOrder = 1
        };

        _context.ProgramEnrollments.Add(enrollment);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}

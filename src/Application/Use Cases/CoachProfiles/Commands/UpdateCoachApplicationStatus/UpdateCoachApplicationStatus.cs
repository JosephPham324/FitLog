using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using FitLog.Application.Common.ValidationRules;
using FitLog.Application.Common.Models;

namespace FitLog.Application.CoachProfiles.Commands.UpdateCoachApplicationStatus;

public record UpdateCoachApplicationStatusCommand : IRequest<Result>
{
    public int ApplicationId { get; init; }
    public string Status { get; init; } = null!;
    public string? StatusReason { get; init; }
    public string UpdatedById { get; init; } = null!;
}

public class UpdateCoachApplicationStatusCommandValidator : AbstractValidator<UpdateCoachApplicationStatusCommand>
{
    public UpdateCoachApplicationStatusCommandValidator()
    {
        RuleFor(x => x.ApplicationId).GreaterThan(0);
        RuleFor(x => x.Status)
                   .NotEmpty()
                   .Must(ValidationRules.BeAValidCoachApplicationStatus)
                   .WithMessage("Invalid status value.");
        RuleFor(x => x.UpdatedById).NotEmpty();
    }
}

public class UpdateCoachApplicationStatusCommandHandler : IRequestHandler<UpdateCoachApplicationStatusCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public UpdateCoachApplicationStatusCommandHandler(IApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<Result> Handle(UpdateCoachApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var application = await _context.CoachApplications
            .Include(ca => ca.Applicant) // Ensure Applicant is loaded
            .FirstOrDefaultAsync(ca => ca.Id == request.ApplicationId, cancellationToken);

        if (application == null)
        {
            throw new NotFoundException(nameof(CoachApplication), request.ApplicationId + "");
        }

        application.Status = request.Status;
        application.StatusReason = request.StatusReason;
        application.LastModified = DateTimeOffset.UtcNow;
        application.LastModifiedBy = request.UpdatedById;

        await _context.SaveChangesAsync(cancellationToken);

        // Send email notification to the applicant
        var emailSubject = "Your Coach Application Status Update";
        var emailBody = $"Dear {application.Applicant.UserName},\n\n" +
                        $"Your coach application status has been updated to: {request.Status}.\n\n" +
                        $"Reason: {request.StatusReason}\n\n" +
                        $"Best regards,\n" +
                        $"The FitLog Team";
        var recepientAddress = application.Applicant.Email;

        await _emailService.SendAsync(recepientAddress ?? "", emailSubject, emailBody);

        return Result.Successful();
    }
}

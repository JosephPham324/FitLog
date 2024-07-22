using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Common.ValidationRules;
using FitLog.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.CoachProfiles.Commands.UpdateCoachApplicationStatus
{
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
        private readonly INotificationService _notificationService;

        public UpdateCoachApplicationStatusCommandHandler(IApplicationDbContext context, IEmailService emailService, INotificationService notificationService)
        {
            _context = context;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        public async Task<Result> Handle(UpdateCoachApplicationStatusCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.CoachApplications
                .Include(ca => ca.Applicant) // Ensure Applicant is loaded
                .FirstOrDefaultAsync(ca => ca.Id == request.ApplicationId, cancellationToken);

            if (application == null)
            {
                throw new NotFoundException(nameof(CoachApplication), request.ApplicationId.ToString());
            }

            application.Status = request.Status;
            application.StatusReason = request.StatusReason;
            application.LastModified = DateTimeOffset.UtcNow;
            application.LastModifiedBy = request.UpdatedById;

            await _context.SaveChangesAsync(cancellationToken);

            // Prepare the notification message
            var notificationMessage = $"Dear {application.Applicant.UserName},\n\n" +
                                      $"Your coach application status has been updated to: {request.Status}.\n\n" +
                                      $"Reason: {request.StatusReason}\n\n" +
                                      $"Best regards,\n" +
                                      $"The FitLog Team";

            // Send notification to the applicant
            await _notificationService.SendNotificationAsync(application.ApplicantId, notificationMessage);

            // Send email notification to the applicant if the email address is available
            if (!string.IsNullOrEmpty(application.Applicant.Email))
            {
                var emailSubject = "Your Coach Application Status Update";
                await _emailService.SendAsync(application.Applicant.Email, emailSubject, notificationMessage);
            }

            return Result.Successful();
        }
    }
}

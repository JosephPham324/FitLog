using FitLog.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Domain.Constants;

namespace FitLog.Application.CoachingServices.Commands.UpdateBookingStatus
{
    public record UpdateBookingStatusCommand(int BookingId, string NewStatus, DateTime? PaymentDate = null) : IRequest<Result>;

    public class UpdateBookingStatusCommandValidator : AbstractValidator<UpdateBookingStatusCommand>
    {
        public UpdateBookingStatusCommandValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty().WithMessage("Booking ID is required.");
            RuleFor(x => x.NewStatus)
                .NotEmpty().WithMessage("New status is required.")
                .Must(ServiceBookingStatus.IsValidStatus).WithMessage("Invalid booking status.");
        }
    }

    public class UpdateBookingStatusCommandHandler : IRequestHandler<UpdateBookingStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public UpdateBookingStatusCommandHandler(IApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Result> Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.CoachingBookings
                .Include(b => b.User)
                .Include(b => b.CoachingService)
                .FirstOrDefaultAsync(b => b.BookingId == request.BookingId, cancellationToken);

            if (booking == null)
            {
                return Result.Failure(new[] { "Booking not found." });
            }

            booking.Status = request.NewStatus;
            if (request.PaymentDate.HasValue)
            {
                booking.PaymentDate = request.PaymentDate.Value;
            }

            _context.CoachingBookings.Update(booking);
            await _context.SaveChangesAsync(cancellationToken);

            var emailSubject = "Booking Status Updated";
            var emailBody = $"Your booking for the service '{booking.CoachingService?.ServiceName}' has been updated to '{request.NewStatus}'.";

            if (booking.User != null && booking.User.Email != null)
            {
                await _emailService.SendAsync(booking.User.Email, emailSubject, emailBody);
            }

            return Result.Successful();
        }
    }
}

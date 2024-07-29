using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.CoachingServices.Queries.GetBookingDetails
{
    public record GetBookingDetailsQuery(int BookingId) : IRequest<object>;

    public class GetBookingDetailsQueryValidator : AbstractValidator<GetBookingDetailsQuery>
    {
        public GetBookingDetailsQueryValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty().WithMessage("Booking ID is required.");
        }
    }

    public class GetBookingDetailsQueryHandler : IRequestHandler<GetBookingDetailsQuery, object>
    {
        private readonly IApplicationDbContext _context;

        public GetBookingDetailsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> Handle(GetBookingDetailsQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.CoachingBookings
                .Include(b => b.User)
                .Include(b => b.CoachingService)
                .FirstOrDefaultAsync(b => b.BookingId == request.BookingId, cancellationToken);

            if (booking == null)
            {
                return Result.Failure(["Booking not found."]);
            }

            var bookingDetails = new
            {
                booking.BookingId,
                booking.UserId,
                UserName = booking.User?.UserName,
                booking.CoachingServiceId,
                ServiceName = booking.CoachingService?.ServiceName,
                booking.Status,
                booking.CreatedDate,
                booking.PaymentDate
            };

            return bookingDetails;
        }
    }
}

using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.CoachingServices.Queries.GetServiceBookings
{
    public record GetServiceBookingsQuery(int ServiceId) : IRequest<object>;

    public class GetServiceBookingsQueryValidator : AbstractValidator<GetServiceBookingsQuery>
    {
        public GetServiceBookingsQueryValidator()
        {
            RuleFor(x => x.ServiceId).NotEmpty().WithMessage("Service ID is required.");
        }
    }

    public class GetServiceBookingsQueryHandler : IRequestHandler<GetServiceBookingsQuery, object>
    {
        private readonly IApplicationDbContext _context;

        public GetServiceBookingsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> Handle(GetServiceBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _context.CoachingBookings
                .Include(b => b.User)
                .Include(b => b.CoachingService)
                .Where(b => b.CoachingServiceId == request.ServiceId)
                .ToListAsync(cancellationToken);

            if (bookings == null || bookings.Count == 0)
            {
                return Result.Failure(["No bookings found for the specified service."]);
            }

            var bookingDetails = bookings.Select(b => new
            {
                b.BookingId,
                b.UserId,
                UserName = b.User?.UserName,
                b.CoachingServiceId,
                ServiceName = b.CoachingService?.ServiceName,
                b.Status,
                b.CreatedDate,
                b.PaymentDate
            });

            return bookingDetails;
        }
    }
}

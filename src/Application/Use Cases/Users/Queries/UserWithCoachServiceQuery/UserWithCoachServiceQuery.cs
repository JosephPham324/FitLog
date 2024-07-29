using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Constants;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Users.Queries.UserWithCoachServiceQuery
{
    public record UserWithCoachServiceQueryQuery : IRequest<bool>
    {
        public string UserId { get; init; } = string.Empty;
        public string CoachId { get; init; } = string.Empty;
    }

    public class UserWithCoachServiceQueryQueryValidator : AbstractValidator<UserWithCoachServiceQueryQuery>
    {
        public UserWithCoachServiceQueryQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.CoachId).NotEmpty().WithMessage("Coach ID is required.");
        }
    }

    public class UserWithCoachServiceQueryQueryHandler : IRequestHandler<UserWithCoachServiceQueryQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public UserWithCoachServiceQueryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UserWithCoachServiceQueryQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _context.CoachingBookings
                    .Include(cb => cb.CoachingService)
                .Where(cb => cb.UserId == request.UserId)
                .ToListAsync();
            if (bookings.IsNullOrEmpty()) return false;

            bool res = bookings
                .Where(cb => cb.CoachingService != null && cb.CoachingService.CreatedBy == request.CoachId
                             && cb.Status == ServiceBookingStatus.Confirmed)
                .Any();

            return res;
        }
    }
}

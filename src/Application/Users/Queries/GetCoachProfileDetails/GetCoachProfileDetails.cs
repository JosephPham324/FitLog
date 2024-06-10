using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Queries.GetCoachProfileDetails;

public record GetCoachProfileDetailsQuery : IRequest<CoachProfileDTO>
{
}

public class GetCoachProfileDetailsQueryValidator : AbstractValidator<GetCoachProfileDetailsQuery>
{
    public GetCoachProfileDetailsQueryValidator()
    {
    }
}

public class GetCoachProfileDetailsQueryHandler : IRequestHandler<GetCoachProfileDetailsQuery, CoachProfileDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCoachProfileDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CoachProfileDTO> Handle(GetCoachProfileDetailsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

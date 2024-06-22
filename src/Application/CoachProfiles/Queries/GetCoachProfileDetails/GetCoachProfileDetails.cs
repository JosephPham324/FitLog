using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.CoachProfiles.Queries.GetCoachProfileDetails;

public record GetCoachProfileDetailsQuery : IRequest<object>
{
}

public class GetCoachProfileDetailsQueryValidator : AbstractValidator<GetCoachProfileDetailsQuery>
{
    public GetCoachProfileDetailsQueryValidator()
    {
    }
}

public class GetCoachProfileDetailsQueryHandler : IRequestHandler<GetCoachProfileDetailsQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetCoachProfileDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(GetCoachProfileDetailsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

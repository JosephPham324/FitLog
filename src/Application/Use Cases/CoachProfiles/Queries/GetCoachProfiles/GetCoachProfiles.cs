using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.CoachProfiles.Queries.GetCoachProfiles;

public record GetCoachProfilesQuery : IRequest<object>
{
}

public class GetCoachProfilesQueryValidator : AbstractValidator<GetCoachProfilesQuery>
{
    public GetCoachProfilesQueryValidator()
    {
    }
}

public class GetCoachProfilesQueryHandler : IRequestHandler<GetCoachProfilesQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetCoachProfilesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(GetCoachProfilesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

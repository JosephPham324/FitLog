using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.CoachProfiles.Queries.UpdateCoachProfile;

public record UpdateCoachProfileQuery : IRequest<object>
{
}

public class UpdateCoachProfileQueryValidator : AbstractValidator<UpdateCoachProfileQuery>
{
    public UpdateCoachProfileQueryValidator()
    {
    }
}

public class UpdateCoachProfileQueryHandler : IRequestHandler<UpdateCoachProfileQuery, object>
{
    private readonly IApplicationDbContext _context;

    public UpdateCoachProfileQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(UpdateCoachProfileQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

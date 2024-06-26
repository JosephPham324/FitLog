using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.CoachProfiles.Queries.GetCoachProfileDetails;

public record GetCoachProfileDetailsQuery : IRequest<CoachProfileDetailsDto>
{
    public string UserId { get; init; }

    public GetCoachProfileDetailsQuery(string userId)
    {
        UserId = userId;
    }
}


public class GetCoachProfileDetailsQueryValidator : AbstractValidator<GetCoachProfileDetailsQuery>
{
    public GetCoachProfileDetailsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");

    }
}

public class GetCoachProfileDetailsQueryHandler : IRequestHandler<GetCoachProfileDetailsQuery, CoachProfileDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public GetCoachProfileDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CoachProfileDetailsDto> Handle(GetCoachProfileDetailsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile == null)
        {
            throw new NotFoundException(nameof(Profile), request.UserId);
        }

        return new CoachProfileDetailsDto
        {
            ProfileId = profile.ProfileId,
            UserId = profile.UserId,
            Bio = profile.Bio,
            ProfilePicture = profile.ProfilePicture,
            MajorAchievements = profile.MajorAchievements,
            GalleryImageLinks = profile.GalleryImageLinks
        };
    }
}

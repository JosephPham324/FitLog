using FitLog.Application.Common.Interfaces;
using FitLog.Application.Use_Cases.CoachProfiles.Queries.GetCoachProfileDetails;

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
    private readonly IMapper _mapper;

    public GetCoachProfileDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CoachProfileDetailsDto> Handle(GetCoachProfileDetailsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        var programs = await _context.Programs.
            Where(p => p.UserId == request.UserId).ToListAsync(cancellationToken);



        if (profile == null)
        {
            throw new NotFoundException(nameof(Profile), request.UserId);
        }

        var profileDto = _mapper.Map<CoachProfileDetailsDto>(profile);
        profileDto.ProgramsOverview = _mapper.Map<IEnumerable<ProgramOverviewDto>>(programs);

        return profileDto;

    }
}

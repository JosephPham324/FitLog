using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.CoachProfiles.Queries.CreateCoachApplication;

public record CreateCoachApplicationQuery : IRequest<bool>
{
    public string? Token { get; init; }
}

public class CreateCoachApplicationQueryValidator : AbstractValidator<CreateCoachApplicationQuery>
{
    public CreateCoachApplicationQueryValidator()
    {
    }
}

public class CreateCoachApplicationQueryHandler : IRequestHandler<CreateCoachApplicationQuery, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserTokenService _tokenService;

    public CreateCoachApplicationQueryHandler(IApplicationDbContext context, IUserTokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<bool> Handle(CreateCoachApplicationQuery request, CancellationToken cancellationToken)
    {
        var userId = _tokenService.GetUserIdFromGivenToken(request.Token??"");


        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var coachApplication = new CoachApplication
        {
            ApplicantId = userId,
            Status = "Pending",
            StatusUpdateTime = DateTime.UtcNow,
            StatusUpdatedById = userId // Assuming the applicant is also the updater initially
        };

        _context.CoachApplications.Add(coachApplication);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

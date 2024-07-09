using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.CoachProfiles.Queries.CreateCoachApplication;

public record CreateCoachApplicationQuery : IRequest<Result>
{
    public string? Token { get; init; }
}

public class CreateCoachApplicationQueryValidator : AbstractValidator<CreateCoachApplicationQuery>
{
    public CreateCoachApplicationQueryValidator()
    {
    }
}

public class CreateCoachApplicationQueryHandler : IRequestHandler<CreateCoachApplicationQuery, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserTokenService _tokenService;

    public CreateCoachApplicationQueryHandler(IApplicationDbContext context, IUserTokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<Result> Handle(CreateCoachApplicationQuery request, CancellationToken cancellationToken)
    {
        var userId = _tokenService.GetUserIdFromGivenToken(request.Token??"");


        if (userId == null)
        {
            return Result.Failure(["User is not authenticated"]);
            //throw new UnauthorizedAccessException("User is not authenticated");
        }

        var coachApplication = new CoachApplication
        {
            ApplicantId = userId,
            Status = "Pending",
            LastModified = DateTime.UtcNow,
            LastModifiedBy = userId // Assuming the applicant is also the updater initially
        };

        _context.CoachApplications.Add(coachApplication);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}

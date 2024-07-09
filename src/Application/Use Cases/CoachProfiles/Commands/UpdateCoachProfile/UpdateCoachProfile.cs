using System.Text.Json;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.CoachProfiles.Commands.UpdateCoachProfile;

public record UpdateCoachProfileCommand : IRequest<Result>
{
    public string UserId { get; init; }
    public string? Bio { get; init; }
    public string? ProfilePicture { get; init; }
    public List<string>? MajorAchievements { get; init; }
    public List<string>? GalleryImageLinks { get; init; }

    public UpdateCoachProfileCommand(string userId, string? bio, string? profilePicture, List<string>? majorAchievements, List<string>? galleryImageLinks)
    {
        UserId = userId;
        Bio = bio;
        ProfilePicture = profilePicture;
        MajorAchievements = majorAchievements;
        GalleryImageLinks = galleryImageLinks;
    }
}

public class UpdateCoachProfileQueryValidator : AbstractValidator<UpdateCoachProfileCommand>
{
    public UpdateCoachProfileQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
    }
}

public class UpdateCoachProfileCommandHandler : IRequestHandler<UpdateCoachProfileCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateCoachProfileCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateCoachProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile == null)
        {
            profile = new Domain.Entities.Profile
            {
                UserId = request.UserId,
                Bio = request.Bio,
                ProfilePicture = request.ProfilePicture,
                MajorAchievements = request.MajorAchievements ?? new List<string>(),
                GalleryImageLinks = request.GalleryImageLinks ?? new List<string>()
            };

            _context.Profiles.Add(profile);
        }
        else
        {
            profile.Bio = request.Bio;
            profile.ProfilePicture = request.ProfilePicture;
            profile.MajorAchievements = request.MajorAchievements ?? new List<string>();
            profile.GalleryImageLinks = request.GalleryImageLinks ?? new List<string>();
        }

        // Update the JSON string representation for GalleryImageLinks
        profile.GalleryImageLinksJson = JsonSerializer.Serialize(profile.GalleryImageLinks);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}

using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries.GetCoachesListWithPagination;

public class CoachSummaryDTO
{
    public string FullName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    public string? MajorAchievement { get; set; }
    public string? InstagramLink { get; set; }
    public string? YouTubeLink { get; set; }
    public string? PatreonLink { get; set; }
    public string? ProgramsCount { get; set; } // Assuming this is the count of programs

    public CoachSummaryDTO(Domain.Entities.Profile profile, AspNetUser user)
    {
        FullName = $"{user.FirstName} {user.LastName}";
        ProfilePicture = profile.ProfilePicture;
        Bio = profile.Bio;
        MajorAchievement = profile.MajorAchievements?.FirstOrDefault(); // Displaying the first major achievement
        InstagramLink = profile.InstagramLink;
        YouTubeLink = profile.YouTubeLink;
        PatreonLink = profile.PatreonLink;
        ProgramsCount = user.Programs.Count.ToString(); // Assuming this is the count of programs
    }
}

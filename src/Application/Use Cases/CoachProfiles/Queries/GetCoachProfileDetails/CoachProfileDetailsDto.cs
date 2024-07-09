using FitLog.Application.Use_Cases.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Application.Users.Queries.GetUserDetails;
using FitLog.Domain.Entities;

namespace FitLog.Application.CoachProfiles.Queries.GetCoachProfileDetails;

public class CoachProfileDetailsDto
{
    public int ProfileId { get; set; }
    public string? UserId { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePicture { get; set; }
    public List<string>? MajorAchievements { get; set; }
    public List<string>? GalleryImageLinks { get; set; }
    public IEnumerable<ProgramOverviewDto>? ProgramsOverview { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Profile, CoachProfileDetailsDto>();
        }
    }
}

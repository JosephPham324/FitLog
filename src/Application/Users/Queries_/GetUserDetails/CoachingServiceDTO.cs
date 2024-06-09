using FitLog.Application.Users.Queries_.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries_.GetUserDetails;

public class CoachingServiceDTO
{
    public int CoachingServiceId { get; set; }
    public string? ServiceName { get; set; }
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public decimal? Price { get; set; }
    public bool? ServiceAvailability { get; set; }
    public string? AvailabilityAnnouncement { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<CoachingService, CoachingServiceDTO>();
        }
    }
}

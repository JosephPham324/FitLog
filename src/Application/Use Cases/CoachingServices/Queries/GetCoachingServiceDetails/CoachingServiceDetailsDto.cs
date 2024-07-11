using AutoMapper.Configuration.Annotations;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.CoachingServices.Queries.GetCoachingServiceDetails;

public class CoachingServiceDetailsDto
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = null!;
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public decimal? Price { get; set; }
    public bool? ServiceAvailability { get; set; }
    public string? AvailabilityAnnouncement { get; set; }
    public DateTimeOffset Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    [Ignore]
    public string? CreatedByUserName { get; set; }
    [Ignore]
    public string? LastModifiedByUserName { get; set; }

    //Automapper mapping profile

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<CoachingService, CoachingServiceDetailsDto>()
            .ForMember(dest => dest.CreatedByUserName, opt => opt.Ignore())
            .ForMember(dest => dest.LastModifiedByUserName, opt => opt.Ignore()); ;
        }
    }
}

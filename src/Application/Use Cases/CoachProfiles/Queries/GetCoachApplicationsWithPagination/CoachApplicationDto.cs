using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.CoachProfiles.Queries.GetCoachApplicationsWithPagination;

public class CoachApplicationDto
{
    public int Id { get; set; }
    public string ApplicantId { get; set; } = "";
    public string Status { get; set; } = "";
    public string? StatusReason { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public string ApplicantName { get; set; } = "";
    public string ApplicantEmail { get; set; } = "";

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<CoachApplication, CoachApplicationDto>()
                .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => src.Applicant.UserName))
                .ForMember(dest => dest.ApplicantEmail, opt => opt.MapFrom(src => src.Applicant.Email));
        }
    }
}

using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries.GetUserDetails;

public class UserProfileDTO
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }

    public List<ProgramDTO> Programs { get; set; } = new List<ProgramDTO>();
    public List<CertificationDTO> Certifications { get; set; } = new List<CertificationDTO>();
    public List<CoachingServiceDTO> CoachingServices { get; set; } = new List<CoachingServiceDTO>();

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<AspNetUser, UserProfileDTO>();
        }
    }
}

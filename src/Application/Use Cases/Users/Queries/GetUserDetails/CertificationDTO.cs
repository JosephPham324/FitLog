using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries.GetUserDetails;

public class CertificationDTO
{
    public int CertificationId { get; set; }
    public string? CertificationName { get; set; }
    public DateOnly? CertificationDateIssued { get; set; }
    public DateOnly? CertificationExpirationData { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<Certification, CertificationDTO>();
        }
    }
}

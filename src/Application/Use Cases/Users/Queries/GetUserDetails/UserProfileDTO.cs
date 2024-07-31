using AutoMapper;
using FitLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitLog.Application.Users.Queries.GetUserDetails
{
    public class UserProfileDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string Roles { get; set; } = string.Empty;

        public List<ProgramDTO> Programs { get; set; } = new List<ProgramDTO>();
        public List<CertificationDTO> Certifications { get; set; } = new List<CertificationDTO>();
        public List<CoachingServiceDTO> CoachingServices { get; set; } = new List<CoachingServiceDTO>();

        private class Mapping : AutoMapper.Profile
        {
            public Mapping()
            {
                CreateMap<AspNetUser, UserProfileDTO>()
                    .ForMember(dest => dest.Roles, opt => opt.MapFrom<RoleStringResolver>());
            }
        }
    }

    public class RoleStringResolver : IValueResolver<AspNetUser, UserProfileDTO, string>
    {
        public string Resolve(AspNetUser source, UserProfileDTO destination, string destMember, ResolutionContext context)
        {
            return string.Join(", ", source.Roles.Select(r => r.Name));
        }
    }
}
